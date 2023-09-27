using System.Drawing;
using System.Numerics;
using NLog;
using OpenGLGameEngine;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Core.Windowing;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Math;
using OpenGLGameEngine.UI;
using OpenGLGameEngine.Universe;

var logger = LogManager.GetCurrentClassLogger();
Console.WriteLine("Hello World!");
Game.Init("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));

// GameWorld.GlobalShader = new Shader(new[] {
//         ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
//         ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
// });

var world = new World();

var testCanvas = world.AddActor(new CanvasActor());

var renderTexture = Texture.CreateEmpty(720, 720, new TextureConfig());
var testRenderTarget = new RenderTarget(renderTexture);
world.AddActor(new TestCameraController() {
        renderTarget = testRenderTarget
});
world.AddActor(new TestCameraController());
var parent = world.AddActor(new TestObject(renderTexture));
var cube = PrimitiveShape.CreateCube();
cube.Mesh.SetTexture(Texture.FromFile("./CheckerboardMap.png", new TextureConfig()));
cube.transform.position = new Vector3(0, 2, 0);
world.AddActor(cube, parent.transform);

WorldManager.LoadWorld(world);
Game.Start();


public class TestObject : MeshRenderer
{
    private Texture renderTex;
    public TestObject(Texture renderTex) { this.renderTex = renderTex; }

    public override void Start()
    {
        transform.position.X = 2f;
        Mesh = new Mesh();
        Mesh.SetVertices(
            new MeshVertex(new Vector3(-1f, 1f, 0f), Color.White, new Vector2(0f, 1f)),
            new MeshVertex(new Vector3(1f, 1f, 0f), Color.White, new Vector2(1f, 1f)),
            new MeshVertex(new Vector3(1f, -1f, 0f), Color.White, new Vector2(1f, 0f)),
            new MeshVertex(new Vector3(-1f, -1f, 0f), Color.White, new Vector2(0f, 0f))
        );

        Mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        Mesh.SetTexture(renderTex);

        // transform.scale = new Vector3(0.5f);
    }

    public override void DrawTick()
    {
        transform.rotation.RotateEuler(Vector3.UnitZ*GameTime.DeltaTime);
    }
}