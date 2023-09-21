using OpenGL;
using NLog;
using OpenGLGameEngine.Core.Drawing;

namespace OpenGLGameEngine.Graphics.Rendering;

public class RenderTarget
{
    static Logger logger = LogManager.GetCurrentClassLogger();
    public static uint currentlyBoundFramebuffer { get; private set; } = 0;

    public static RenderTarget Default = new(0);

    /// <summary>
    /// Generates a new render target with an empty framebuffer with any attachements. Pretty useless by itself.
    /// </summary>
    /// <returns></returns>
    public static RenderTarget CreateEmpty() { return new RenderTarget(Gl.GenFramebuffer()); }

    /// <summary>
    /// Creates a new render target with the specified texture attached to it.
    /// </summary>
    /// <returns></returns>
    public static RenderTarget Create(Texture texture, DepthBuffer depthBuffer)
    {
        var target = RenderTarget.CreateEmpty();
        target.Bind();
        texture.Bind();
        Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2d, texture.texId, 0);
        depthBuffer.Bind();
        Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
            RenderbufferTarget.Renderbuffer, depthBuffer.depthBufferId);

        if (!target.Verify()) throw new InvalidOperationException("Failed to create render target!");
        Default.Bind();
        return target;
    }

    public uint fbo { get; }

    public RenderTarget(uint fbo) { this.fbo = fbo; }

    public void Bind()
    {
        currentlyBoundFramebuffer = fbo;
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
    }

    public void Clear() { Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); }

    public bool CheckComplete()
    {
        if (currentlyBoundFramebuffer != fbo)
            throw new InvalidOperationException(
                "Cannot check completeness of a framebuffer that is not currently bound!");
        return Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferStatus.FramebufferComplete;
    }

    /// <summary>
    /// Verifies render target and checks for completeness.<br/>
    /// This should be called after all attachments have been added.<br/>
    /// Avoid calling this method every frame, as it is can be very slow.<br/>
    /// This method will not throw an exception if the framebuffer is not complete. Instead, it will log an error.
    /// </summary>
    public bool Verify()
    {
        Bind();
        var status = Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != FramebufferStatus.FramebufferComplete) logger.Error($"Framebuffer {fbo} is not complete! Status {status}");
        return status == FramebufferStatus.FramebufferComplete;
    }
}