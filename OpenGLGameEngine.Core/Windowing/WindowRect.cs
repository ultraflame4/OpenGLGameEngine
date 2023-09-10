﻿using System.Numerics;

namespace OpenGLGameEngine.Core.Windowing;

public struct WindowRect
{
    public Vector2 position { get; } = Vector2.Zero;
    public Vector2 size { get; }
    
    public float aspectRatio => size.X / size.Y;
    public int Width => (int)size.X;
    public int Height => (int)size.Y;
    public int X => (int)position.X;
    public int Y => (int)position.Y;
    
    public WindowRect(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
    }

    public override string ToString()
    {
        return $"<{GetType().FullName} Position: {position}; Size: {size}>";
    }

    
}