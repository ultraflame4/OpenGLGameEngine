using System.Drawing;
using System.Numerics;
using GLFW;
using NLog;
using OpenGLGameEngine;
using OpenGLGameEngine.Components;
using OpenGLGameEngine.Components.Mesh;
using OpenGLGameEngine.Core.Graphics;

var logger = LogManager.GetCurrentClassLogger();
Console.WriteLine("Hello World!");
Game.CreateMainWindow("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));

var world = Game.GetCurrentWorld();
// GameWorld.GlobalShader = new Shader(new[] {
//         ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
//         ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
// });

world.CreateMainCamera();

world.AddEntityObject(new TestObject());
Game.Start();

public class TestObject : EntityObject
{
    private Mesh mesh;

    public override void Load()
    {
        mesh = Entity.AddComponent(new Mesh(transform, true));
        var texture = new Texture(new Bitmap("./CheckerboardMap.png"));
        mesh.SetVertices(
            new MeshVertex(new Vector3(-1f, 1f, 0f), Color.Red, new Vector2(0f, 1f)),
            new MeshVertex(new Vector3(1f, 1f, 0f), Color.Lime, new Vector2(1f, 1f)),
            new MeshVertex(new Vector3(1f, -1f, 0f), Color.Blue, new Vector2(1f, 0f)),
            new MeshVertex(new Vector3(-1f, -1f, 0f), Color.Blue, new Vector2(0f, 0f))
        );

        mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        mesh.SetTexture(texture);

        transform.scale = new Vector3(0.5f);
    }

    public override void Start() { }

    public override void Update() { }

    public override void Draw()
    {
        var testTransform = Entity?.GetComponent<Transform>();
        if (testTransform == null) return;
        testTransform.rotation.X = (float)Glfw.Time * 2f;
        testTransform.rotation.Y = (float)Glfw.Time * 1.25f;
        testTransform.rotation.Z = (float)Glfw.Time;
    }
}