using System.Numerics;
using NLog;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Components.Camera;

public class Camera : Component
{
   
    public ICameraProjection Projection;
    public Matrix4x4 projMatrix => Projection.GetProjMatrix();
    public Matrix4x4 viewMatrix;
    public Transform transform;

    Logger logger = LogManager.GetCurrentClassLogger();
    public Camera()
    {   
        UsePerspective();
    }


    public bool Enabled { get; set; } = true;

    public override void OnAdd()
    {
        if (Entity==null) return;
        transform = Entity.GetComponent<Transform>();
        if (transform==null) return;
        transform.position = new Vector3(0f, 0f, 3f);
        transform.rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 180f.ToRad());
    }
    public override void OnRemove() { }



    public void UseOrthographic(float size=1f, float zNear = .1f, float zFar = 100f)
    {
        Projection = new OrthographicProjection(size, zNear, zFar);
    }
    
    /// <summary>
    ///     Sets the camera to use a perspective projection
    /// </summary>
    /// <param name="fov">Field of view of camera in radians. (Default is ~1.5708 rads or 90 degrees)</param>
    /// <param name="aspectRatio">Aspect ratio of the camera. Defaults to the window aspect ratio</param>
    /// <param name="zNear">Z near clipping plane. Defaults to 0.1</param>
    /// <param name="zFar">Z far clipping plane. Defaults to 100</param>
    public void UsePerspective(float? fov = null,float size = 1f, float zNear = .1f, float zFar = 100f)
    {
        Projection = new PerspectiveProjection(fov,size, zNear, zFar);
    }

    public void CalcViewMatrix()
    {
        viewMatrix = Matrix4x4.CreateLookAt(transform.position, transform.position+transform.Forward, Vector3.UnitY);
    }

}