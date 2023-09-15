﻿using System.Numerics;

namespace OpenGLGameEngine.Math;

public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point((int x, int y) xy)
    {
        X = xy.x;
        Y = xy.y;
    }

    public Vector2 toVector() { return new Vector2(X, Y); }
    public (int x, int y) toXY() { return (X, Y); }
    public static implicit operator Vector2 (Point p) { return p.toVector(); }
    public static implicit operator (int x, int y) (Point p) { return p.toXY(); }
    public override string ToString() => $"({X}, {Y})";
}