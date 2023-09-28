using System.Numerics;
using NLog;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class CanvasRenderer : Actor, IRenderable
{
    private readonly Mesh Mesh;
    private readonly Logger logger = LogManager.GetCurrentClassLogger();
    public CanvasRenderer()
    {
        Mesh = MeshUtils.CreateQuad(Vector2.One*2);
        Mesh.SetTexture(Texture.CreateEmpty(100,100, new TextureConfig()));
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