using System.Numerics;
using OpenGLGameEngine.Math;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Components.Camera;

public class PerspectiveProjection : ICameraProjection
{
    public static float DEFAULT_FOV = 45f.ToRadians();
    public float size { get; set; }
    public float zNear { get; set; }
    public float zFar { get; set; }
    public float fov { get; set; }


    public PerspectiveProjection(float? fov = null, float size = 1f, float zNear = .1f, float zFar = 100f)
    {
        this.fov = fov ?? DEFAULT_FOV;
        this.zNear = zNear;
        this.zFar = zFar;
        this.size=size;
    }


    public Matrix4x4 GetProjMatrix()
    {
        return Matrix4x4.CreatePerspectiveFieldOfView(fov, WindowUtils.GetAspectRatio() * size, 0.01f, 100f);
    }
}