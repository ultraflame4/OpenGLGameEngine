using System.Numerics;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Components;

public class Camera : Component
{
    public static double DEFAULT_FOV = Utils.Utils.Deg2Rad(45);
    public Matrix4x4 projMatrix;
    public Matrix4x4 viewMatrix;
    public Transform transform;

    public Camera()
    {
        UsePersepective();
    }


    public bool Enabled { get; set; } = true;

    public override void OnAdd()
    {
        if (Entity==null) return;
        transform = Entity.GetComponent<Transform>();
        if (transform==null) return;
        transform.position = new Vector3(0f, 0f, 3f);
        transform.rotation = -1 * Vector3.UnitZ;
    }
    public override void OnRemove() { }


    /// <summary>
    ///     Sets the camera to use a orthographic projection
    /// </summary>
    /// <param name="width">Width of the projection</param>
    /// <param name="height">Height of the projection</param>
    /// <param name="zNear">Z near clipping plane. Defaults to 0.1</param>
    /// <param name="zFar">Z far clipping plane. Defaults to 100</param>
    public void UseOrthographic(float width, float height, float zNear = .1f, float zFar = 100f)
    {
        projMatrix = Matrix4x4.CreateOrthographic(width, height, zNear, zFar);
    }

    /// <summary>
    ///     Sets the camera to use a perspective projection
    /// </summary>
    /// <param name="width">Width of the projection</param>
    /// <param name="height">Height of the projection</param>
    /// <param name="zNear">Z near clipping plane. Defaults to 0.1</param>
    /// <param name="zFar">Z far clipping plane. Defaults to 100</param>
    public void UsePersepective(float width, float height, float zNear = .1f, float zFar = 100f)
    {
        projMatrix = Matrix4x4.CreatePerspective(width, height, zNear, zFar);
    }

    /// <summary>
    ///     Sets the camera to use a perspective projection
    /// </summary>
    /// <param name="fov">Field of view of camera in radians. (Default is ~1.5708 rads or 90 degrees)</param>
    /// <param name="aspectRatio">Aspect ratio of the camera. Defaults to the window aspect ratio</param>
    /// <param name="zNear">Z near clipping plane. Defaults to 0.1</param>
    /// <param name="zFar">Z far clipping plane. Defaults to 100</param>
    public void UsePersepective(double? fov = null, float? aspectRatio = null, float zNear = .1f, float zFar = 100f)
    {
        projMatrix = Matrix4x4.CreatePerspectiveFieldOfView((float)(fov ?? DEFAULT_FOV), aspectRatio ?? WindowUtils.GetAspectRatio(), 0.01f, 100f);
    }

    public void CalcViewMatrix()
    {
        viewMatrix = Matrix4x4.CreateLookAt(transform.position, transform.position+transform.rotation, Vector3.UnitY);
    }
}