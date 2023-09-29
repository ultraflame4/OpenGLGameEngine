using OpenGL;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Graphics.LowLevel;

public class RenderBuffer : IGLObj
{
    public uint id { get; }
    public InternalFormat renderBufferType { get; private set; }
    public RenderBuffer(uint id) { this.id = id; }
    
    
    public static RenderBuffer CreateEmpty()=> new RenderBuffer(Gl.GenRenderbuffer());

    public static RenderBuffer Create(int width, int height, InternalFormat renderBufferType)
    {
        var buffer = CreateEmpty();
        buffer.Bind();
        buffer.renderBufferType = renderBufferType;
        Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, renderBufferType, width, height);
        return buffer;
    }
    
    /// <summary>
    /// Creates a depth buffer with the given width and height.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static RenderBuffer CreateDepth(int width, int height) => Create(width, height, InternalFormat.DepthComponent);

    public void Bind()
    {
        Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, id);
    }

    public void Resize(Point size)
    {
        Bind();
        Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, renderBufferType, size.X, size.Y);
    }
}