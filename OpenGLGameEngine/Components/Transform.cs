using System.Numerics;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Components;

public class Transform : Component
{
    public Transform? parent;
    public Vector3 position = Vector3.Zero;
    /**
     * 
     * NOTICE / WARNING: WE ARE USING A VECTOR HERE TO REPRESENT DIRECTION
     * TODO WE HAVE TO IN THE FUTURE EITHER PROVIDE A FUNCTION TO ROTATE IT OR SWITCH TO QUATERNIONS.
     * (Because we should not be manipulating the xyz directly, because if we keep adding to y for example,
     * the camera will never flip around to the back because that would be negative and we are only adding. So we either do a custom function to detect if y>1
     * and set y=-1 or do something else) 
     */
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