
using System.Drawing;
using OpenGL;
using NLog;
using OpenGLGameEngine.Graphics.LowLevel;
using Point = OpenGLGameEngine.Math.Point;

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
    
    public uint fbo { get; }
    public readonly Texture? texture = null;
    public readonly RenderBuffer depthBuffer = null;
    public GlColor ClearColor = GlColor.FromColor(Color.FromArgb(0x00));
    
    private FramebufferTarget framebufferType = FramebufferTarget.Framebuffer;
    public RenderTarget(uint fbo) { this.fbo = fbo; }

    public RenderTarget(Texture texture, bool depth = true)
    {
        fbo = Gl.GenFramebuffer();
        Bind();
        this.texture = texture;
        depthBuffer = RenderBuffer.CreateDepth(texture.width,texture.height);
        this.texture.Bind();
        Gl.FramebufferTexture2D(framebufferType, FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2d, texture.id, 0);
        depthBuffer.Bind();
        Gl.FramebufferRenderbuffer(framebufferType, FramebufferAttachment.DepthAttachment,
            RenderbufferTarget.Renderbuffer, depthBuffer.id);
        
        if (!Verify()) throw new InvalidOperationException("Failed to create render target!");
        Default.Bind();
    }

    public void Resize(Point size)
    {
        if (texture == null) return;
        texture.Resize(size);
        depthBuffer.Resize(size);
    }
    public void Bind()
    {
        currentlyBoundFramebuffer = fbo;
        Gl.BindFramebuffer(framebufferType, fbo);
    }

    public void Clear()
    {
        Gl.ClearColor(ClearColor.r, ClearColor.g, ClearColor.b , ClearColor.a);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
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
        var status = Gl.CheckFramebufferStatus(framebufferType);
        if (status != FramebufferStatus.FramebufferComplete) logger.Error($"Framebuffer {fbo} is not complete! Status {status}");
        return status == FramebufferStatus.FramebufferComplete;
    }
}