using System.Numerics;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Universe;

public class TransformNode : Transform
{
    public TransformNode? Parent = null;
    public List<TransformNode> Children { get; } = new();
    public Matrix4x4 GetModelMatrix()
    {
        return Parent == null ? TransformMatrix : Parent.GetModelMatrix() * TransformMatrix;
    }
}