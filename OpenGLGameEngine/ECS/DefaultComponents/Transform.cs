using System.Numerics;

namespace OpenGLGameEngine.ECS.DefaultComponents;

public class Transform : IComponent
{
    public Vector3 position = new Vector3(0, 0, -1f);
    public Vector3 rotation = Vector3.Zero;
    public Vector3 scale = Vector3.One;
    public Transform? parent;
    public void OnAdd() { }

    public void OnRemove() { }


    public Matrix4x4 TransformMatrix =>
            Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
            Matrix4x4.CreateScale(scale) *
            Matrix4x4.CreateTranslation(position);

    public Matrix4x4 GetModelMatrix()
    {
        return parent is null ? TransformMatrix : parent.GetModelMatrix() * TransformMatrix;
    }
}