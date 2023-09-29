using System.Numerics;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Rendering;

namespace OpenGLGameEngine.UI;
/// <summary>
/// Renders the contents of the canvas to a texture.
/// </summary>
public class CanvasTarget : IRenderable, IRenderCamera
{
    public Matrix4x4 projMatrix { get; } = Matrix4x4.Identity;
    public Matrix4x4 viewMatrix { get; } = Matrix4x4.Identity;
    public RenderTarget renderTarget { get; }
    public HashSet<string> layers { get; set; } = new HashSet<string>() { "ui" };
    public readonly Texture renderTex;
    public UIElement rootElement;

    public CanvasTarget(UIElement rootElement)
    {
        this.rootElement = rootElement;
        renderTex = Texture.CreateEmpty(700,700, new TextureConfig());
        renderTarget = new RenderTarget(renderTex);
        RenderPipeline.renderables.Add(this);
        RenderPipeline.cameras.Add(this );
    }

    public void Render(IRenderCamera camera)
    {
        rootElement.Render();
    }
}