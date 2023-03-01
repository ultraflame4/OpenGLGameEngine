using System.Numerics;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Components;

public class Transform : IComponent
{
    public Transform? parent;
    public Vector3 position = new(0, 0, -1f);
    public Vector3 rotation = Vector3.Zero;
    public Vector3 scale = Vector3.One;

    public Matrix4x4 TransformMatrix =>
            Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
            Matrix4x4.CreateScale(scale) *
            Matrix4x4.CreateTranslation(position);

    public void OnAdd() { }

    public void OnRemove() { }

    public bool Enabled { get; set; } = true;

    public Matrix4x4 GetModelMatrix()
    {
        return parent is null ? TransformMatrix : parent.GetModelMatrix() * TransformMatrix;
    }
}