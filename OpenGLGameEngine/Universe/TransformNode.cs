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

    /// <summary>
    /// Sets the parent of this node. Will remove this node from the old parent and add it to the new parent.
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(TransformNode? parent)
    {
        Parent?.Children.Remove(this);
        Parent = parent;
        parent?.Children.Add(this);
    }

    /// <summary>
    /// Adds a child to this node. Same as child.SetParent(this)
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(TransformNode child)
    {
        child.SetParent(this);
    }
}