using System.Numerics;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.UI;
/// <summary>
/// Renders the contents of the canvas to a texture.
/// </summary>
public class CanvasTarget : IRenderable
{
    public RenderTarget renderTarget { get; }
    public HashSet<string> layers { get; set; } = new() { "ui" };
    public readonly Texture renderTex;
    public UIElement rootElement;

    public CanvasTarget(UIElement rootElement)
    {
        this.rootElement = rootElement;
        renderTex = Texture.CreateEmpty(RenderPipeline.window.CurrentRect.size.X,RenderPipeline.window.CurrentRect.size.Y, new TextureConfig());
        renderTarget = new RenderTarget(renderTex);
        RenderPipeline.renderables.Add(this);
        
    }

    public void Resize(Point size)
    {
        renderTarget.Resize(size);
        // rootElement.SetSize(size);
    }

    public void Render(IRenderCamera camera)
    {
        rootElement.Render(camera);
    }
}