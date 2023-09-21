using OpenGL;

namespace OpenGLGameEngine.Core.Drawing;

public class DepthBuffer
{
    
    public static DepthBuffer Create(int width, int height)
    {
        var id = Gl.GenRenderbuffer();
        Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, id);
        Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent, width, height);
        return new DepthBuffer(id);
    }
    
    public readonly uint depthBufferId;
    
    public DepthBuffer(uint depthBufferId) { this.depthBufferId = depthBufferId; }
    
    public void Bind()
    {
        Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBufferId);
    }
}