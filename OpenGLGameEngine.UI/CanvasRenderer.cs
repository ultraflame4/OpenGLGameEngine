using System.Drawing;
using System.Numerics;
using NLog;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;
/// <summary>
/// Renders the canvas to the world.
/// </summary>
public class CanvasRenderer : Actor, IRenderable
{
    private readonly Mesh Mesh;
    private readonly CanvasTarget canvasTarget;
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    public CanvasRenderer()
    {
        canvasTarget = new CanvasTarget(new UIElement());
        Mesh = MeshUtils.CreateQuad(Vector2.One*2, color: Color.Green);
        Mesh.SetTexture(canvasTarget.renderTex);
        RenderPipeline.renderables.Add(this);
    }

    public void Resize(Vector2 size)
    {
        Mesh.SetVertices(MeshUtils.GetQuadVertices(size));
    }

    public HashSet<string> layers { get; } = RenderPipeline.GetDefaultLayers();

    public void Render(IRenderCamera camera)
    {
        var shader = MeshRenderer.defaultShader;
        if (shader == null)
        {
            logger.Error("No shader! Cannot render mesh!");
            return;
        }
        shader.Use();
        // Ignore model, view and projection matrices because we want to render in screen space (clip space)
        shader.SetUniform("model",  Matrix4x4.Identity); 
        shader.SetUniform("view", Matrix4x4.Identity);
        shader.SetUniform("projection",  Matrix4x4.Identity);
        Mesh.Draw();
    }
}