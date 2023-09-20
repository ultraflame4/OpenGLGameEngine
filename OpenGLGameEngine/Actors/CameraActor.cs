using System.Numerics;
using NLog;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Core.Drawing;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Math;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.Actors;

public class CameraActor : Actor, IRenderCamera
{

    public ICameraProjection Projection;
    public Matrix4x4 projMatrix => Projection.GetProjMatrix();
    public Matrix4x4 viewMatrix => Matrix4x4.CreateLookAt(transform.position, transform.position+transform.Forward, Vector3.UnitY);

    Logger logger = LogManager.GetCurrentClassLogger();
    public RenderTarget renderTarget  { get; set; } = RenderTarget.Default;

    public CameraActor()
    {   
        UsePerspective();
        transform.position = new Vector3(0f, 0f, 3f);
        transform.rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 180f.ToRad());
        RenderPipeline.cameras.Add(this);
    }

    public void UseOrthographic(float size=1f, float zNear = .1f, float zFar = 100f)
    {
        Projection = new OrthographicProjection(size, zNear, zFar);
    }
    
    /// <summary>
    ///     Sets the camera to use a perspective projection
    /// </summary>
    /// <param name="fov">Field of view of camera in radians. (Default is ~1.5708 rads or 90 degrees)</param>
    /// <param name="zNear">Z near clipping plane. Defaults to 0.1</param>
    /// <param name="zFar">Z far clipping plane. Defaults to 100</param>
    public void UsePerspective(float? fov = null,float size = 1f, float zNear = .1f, float zFar = 100f)
    {
        Projection = new PerspectiveProjection(fov,size, zNear, zFar);
    }
  
}