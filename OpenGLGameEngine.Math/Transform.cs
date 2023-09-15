using System.Numerics;

namespace OpenGLGameEngine.Math;

public class Transform
{
    public Vector3 position = Vector3.Zero;
    public Vector3 scale = Vector3.One;
    public Quaternion rotation = Quaternion.Identity;

    public Vector3 Up => Vector3.Transform(Vector3.UnitY, rotation);
    public Vector3 Down => -Up;
    public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, rotation);
    public Vector3 Backward => -Forward;
    public Vector3 Right => Vector3.Transform(Vector3.UnitX, rotation);
    public Vector3 Left => -Right;
    
    /// <summary>
    ///    Returns a new transform that is the result of the parent transform applied to this transform.
    ///    Eg. If the parent transform is the world. this.resolve(world) will result in a transform that is in world space.
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public Transform resolve(Transform parent)
    {
        return new Transform
        {
            position = parent.position + position,
            rotation = parent.rotation * rotation,
            scale = parent.scale * scale
        };
    }
    
    public Transform relativeTo(Transform parent)
    {
        return new Transform
        {
            position = position - parent.position,
            rotation = parent.rotation / rotation,
            scale = scale / parent.scale
        };
    }
    public Matrix4x4 TransformMatrix =>
            Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
            Matrix4x4.CreateScale(scale) *
            Matrix4x4.CreateTranslation(position);

    public override string ToString()
    {
        return $"<{GetType().FullName} xy:{position}, rot:{rotation.ToEuler().ToDeg()}, scale:{scale}>";
    }
}