using System.Numerics;

namespace OpenGLGameEngine.Components.Camera;

public interface ICameraProjection
{
    public float size { get; set; }
    public float zNear  { get; set; }
    public float zFar  { get; set; }

    public Matrix4x4 GetProjMatrix();
}