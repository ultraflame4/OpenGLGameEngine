using System.Numerics;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Components.Camera;

public class OrthographicProjection: ICameraProjection
{
    public float size { get; set; }
    public float zNear { get; set; }
    public float zFar { get; set; }
    public OrthographicProjection(float size, float zNear, float zFar)
    {
        this.size = size;
        this.zNear = zNear;
        this.zFar = zFar;
    }

    public Matrix4x4 GetProjMatrix()
    {
        (int width, int height) = WindowUtils.GetWindowSize();
        return Matrix4x4.CreateOrthographic(width*size, height*size, zNear, zFar);
    }
}