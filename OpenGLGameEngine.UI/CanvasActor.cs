using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Core.Drawing;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class CanvasActor : Actor
{
    public readonly CameraActor camera;
    public readonly RenderTarget renderTarget;
    public readonly Texture texture;
    

    public CanvasActor()
    {
        texture = Texture.CreateEmpty(720, 720, new TextureConfig());
        renderTarget = new RenderTarget(texture, DepthBuffer.Create(texture.width, texture.height));
        camera = new CameraActor() {
                renderTarget = renderTarget,
                Projection = new OrthographicProjection()
        };
    }

    public override void Load()
    {
        World.AddActor(camera,transform);
    }
}