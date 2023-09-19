﻿using System.Numerics;
using OpenGLGameEngine.Core;

namespace OpenGLGameEngine.Graphics.Camera;

public class OrthographicProjection: ICameraProjection
{
    public float size { get; set; }
    public float zNear { get; set; }
    public float zFar { get; set; }
    public OrthographicProjection(float size, float zNear, float zFar)
    {
        this.size = size;
        this.zNear = zNear;
        this.zFar = zFar;
    }

    public Matrix4x4 GetProjMatrix()
    {
        Vector2 windowSize = MainWindow.window.CurrentRect.size;
        return Matrix4x4.CreateOrthographic(windowSize.X*size, windowSize.Y*size, zNear, zFar);
    }
}