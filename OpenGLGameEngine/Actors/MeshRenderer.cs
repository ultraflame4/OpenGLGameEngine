using NLog;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.Actors;

public class MeshRenderer : Actor, IRenderable
{
    public static Shader? defaultShader;
    private Logger logger = LogManager.GetCurrentClassLogger();
    public Mesh Mesh = new ();
    public HashSet<string> layers { get; set; } = RenderPipeline.GetDefaultLayers();

    public MeshRenderer()
    {
        RenderPipeline.renderables.Add(this);
    }
    
    public void Render(IRenderCamera camera)
    {
        var shader = Mesh.Shader ?? defaultShader;
        if (shader == null)
        {
            logger.Error("No shader! Cannot render mesh!");
            return;
        }
        shader.Use();
        shader.SetUniform("model", transform.GetModelMatrix());
        shader.SetUniform("projection", camera.projMatrix);
        shader.SetUniform("view", camera.viewMatrix);
        Mesh.Draw();
    }   
}