using System.Drawing;
using System.Numerics;
using NLog;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class CanvasActor : Actor, IRenderable
{
    public readonly CameraActor camera;
    public readonly RenderTarget renderTarget;
    public readonly Texture texture;
    public readonly Mesh mesh;

    private Logger logger = LogManager.GetCurrentClassLogger();
    public HashSet<string> layers { get; } = new (){"ui"};
    
    public CanvasActor()
    {

        texture = Texture.CreateEmpty(720, 720, new TextureConfig());
        renderTarget = new RenderTarget(texture);
        camera = new CameraActor() {
                renderTarget = renderTarget,
                Projection = new OrthographicProjection(),
                layers = new() {"ui"}
        };
        mesh = new Mesh();
        mesh.SetVertices(
            new MeshVertex(new Vector3(-1,-1,0), Color.Black, Vector2.Zero),
            new MeshVertex(new Vector3(1,-1,0), Color.Black, Vector2.UnitX),
            new MeshVertex(new Vector3(-1,1,0), Color.Black, Vector2.UnitY),
            new MeshVertex(new Vector3(1,1,0), Color.Black, Vector2.UnitY + Vector2.UnitX)
        );
        mesh.SetTriangles(0, 1, 2, 1, 3, 2);
        mesh.SetTexture(texture);
        RenderPipeline.renderables.Add(this);
    }

    public override void Load() { World.AddActor(camera, transform); }



    public void Render(IRenderCamera _)
    {
        var shader = MeshRenderer.defaultShader;
        if (shader == null)
        {
            logger.Error("No shader! Cannot render canvas mesh!");
            return;
        }
        shader.Use();
        shader.SetUniform("model", transform.GetModelMatrix());
        shader.SetUniform("projection", camera.projMatrix);
        shader.SetUniform("view", camera.viewMatrix);
        mesh.Draw();
    }
}