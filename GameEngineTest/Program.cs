using System.Drawing;
using System.Numerics;
using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Utils;

using OpenGLGameEngine.Components;
using OpenGLGameEngine.Components.Mesh;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Core.Graphics;
using OpenGLGameEngine.Processors;


namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();
 
    public class TestScript : EntityScript
    {
        public override void Load()
        {
                
        }

        public override void Start()
        {
            logger.Info("TestScript Start");
        }

        public override void Update()
        {
            
        }

        public override void Draw()
        {
            var testTransform = Entity?.GetComponent<Transform>();
            if (testTransform == null) return;
            testTransform.rotation.X = (float)Glfw.Time * 2f;
            testTransform.rotation.Y = (float)Glfw.Time * 1.25f;
            testTransform.rotation.Z = (float)Glfw.Time;
        }

        public override void Remove()
        {
                
        }
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.CreateMainWindow("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));
        
        var world = Game.GetCurrentWorld();
        // GameWorld.GlobalShader = new Shader(new[] {
        //         ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
        //         ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        // });
        
        world.CreateMainCamera();
        
        var testEntity = world.CreateEntity();
        
        var testTransform = testEntity.AddComponent(new Transform());
        var mesh = testEntity.AddComponent(new Mesh(testTransform,true));
        var testScript = testEntity.AddComponent(new TestScript());

        
        testTransform.scale = new Vector3(0.5f);
        var texture = new Texture(new Bitmap("./CheckerboardMap.png"));
        mesh.SetVertices(
                new MeshVertex(new Vector3(-1f, 1f, 0f),Color.Red, new Vector2(0f, 1f)),
                new MeshVertex(new Vector3(1f, 1f, 0f),Color.Lime, new Vector2(1f, 1f)),
                new MeshVertex(new Vector3(1f, -1f, 0f),Color.Blue, new Vector2(1f, 0f)),
                new MeshVertex(new Vector3(-1f, -1f, 0f),Color.Blue, new Vector2(0f, 0f))
        );
        
        mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        mesh.SetTexture(texture);
        
        Game.Start();
    }
}