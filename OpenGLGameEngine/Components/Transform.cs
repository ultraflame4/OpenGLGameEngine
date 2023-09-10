using System.Numerics;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Math;

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
    public Quaternion rotation = Quaternion.Identity;
    public Vector3 scale = Vector3.One;

    public Matrix4x4 TransformMatrix =>
            Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
            Matrix4x4.CreateScale(scale) *
            Matrix4x4.CreateTranslation(position);

    public bool Enabled { get; set; } = true;
    
    public Vector3 Up => Vector3.Transform(Vector3.UnitY, rotation);
    public Vector3 Down => -Up;
    public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, rotation);
    public Vector3 Backward => -Forward;
    public Vector3 Right => Vector3.Transform(Vector3.UnitX, rotation);
    public Vector3 Left => -Right;
    

    public override void OnAdd() { }

    public override void OnRemove() { }

    public Matrix4x4 GetModelMatrix()
    {
        return parent is null ? TransformMatrix : parent.GetModelMatrix() * TransformMatrix;
    }

    public override string ToString()
    {
        return $"<{GetType().FullName}: Position: {position}, Rotation: {rotation.ToDegrees()}, Scale: {scale}>";
    }
}