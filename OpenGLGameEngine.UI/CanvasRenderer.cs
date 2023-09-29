using System.Drawing;
using System.Numerics;
using NLog;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;
using Point = OpenGLGameEngine.Math.Point;

namespace OpenGLGameEngine.UI;
/// <summary>
/// Renders the canvas to the world.
/// </summary>
public class CanvasRenderer : Actor, IRenderable
{
    private readonly Mesh Mesh;
    private readonly CanvasTarget canvasTarget;
    private readonly Logger logger = LogManager.GetCurrentClassLogger();
    private CameraActor camera;

    public CanvasRenderer()
    {
        canvasTarget = new CanvasTarget(new UIElement());
        Mesh = MeshUtils.CreateQuad(Vector2.One);
        Mesh.SetTexture(canvasTarget.renderTex);
        RenderPipeline.renderables.Add(this);
        RenderPipeline.window!.WindowResizedEvent += (w)=>Resize(w.CurrentRect.size);
    }

    public override void Load()
    {
        base.Load();
        camera = new CameraActor() {
                layers = new HashSet<string>() { "ui" },
                Projection = new OrthographicProjection(),
                renderTarget = canvasTarget.renderTarget
        };
        World.AddActor(camera);
    }

    public void Resize(Point size)
    {
       canvasTarget.Resize(size);
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