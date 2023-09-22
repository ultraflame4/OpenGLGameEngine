using System.Numerics;

namespace OpenGLGameEngine.Graphics.Rendering;

public interface IRenderCamera
{
    public Matrix4x4 projMatrix { get; }
    public Matrix4x4 viewMatrix { get; }
    public RenderTarget renderTarget { get;}
    public HashSet<string> layers { get; set; }
}