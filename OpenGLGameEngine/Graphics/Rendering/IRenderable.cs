﻿namespace OpenGLGameEngine.Graphics.Rendering;

public interface IRenderable
{
    public HashSet<string> layers { get;}
    /// <summary>
    /// Called when the object should be drawn to the render target. Could be called multiple times per frame.
    /// </summary>
    /// <param name="camera"></param>
    void Render(IRenderCamera camera);
}