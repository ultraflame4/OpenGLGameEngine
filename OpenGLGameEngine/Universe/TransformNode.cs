﻿using System.Numerics;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Universe;

public class TransformNode : Transform
{
    public TransformNode? Parent = null;
    public List<TransformNode> Children { get; } = new();
    public Matrix4x4 GetModelMatrix()
    {
        return Parent == null ? TransformMatrix : TransformMatrix * Parent.GetModelMatrix();
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
    
    /// <summary>
    /// Removes a child from this node. Same as child.SetParent(null).
    /// </summary>
    /// <param name="child"></param>
    public virtual void RemoveChild(TransformNode child)
    {
        child.SetParent(null);
    }

    public virtual void Destroy()
    {
        
    }
}