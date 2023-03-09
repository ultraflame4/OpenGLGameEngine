using System.Numerics;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Components;

public class Transform : Component
{
    public Transform? parent;
    public Vector3 position = Vector3.Zero;
    public Vector3 rotation = Vector3.Zero;
    public Vector3 scale = Vector3.One;

    public Matrix4x4 TransformMatrix =>
            Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
            Matrix4x4.CreateScale(scale) *
            Matrix4x4.CreateTranslation(position);

    public bool Enabled { get; set; } = true;

    public override void OnAdd() { }

    public override void OnRemove() { }

    public Matrix4x4 GetModelMatrix()
    {
        return parent is null ? TransformMatrix : parent.GetModelMatrix() * TransformMatrix;
    }
}