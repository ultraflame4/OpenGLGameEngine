using OpenGL;

namespace OpenGLGameEngine.Graphics.Rendering;

public class RenderTarget
{
    public uint fbo { get; }
    
    public static RenderTarget Default = new (0);

    public RenderTarget(uint fbo) { this.fbo = fbo; }

    public void Bind()
    {
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
    }
    public void Clear() { Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); }
}