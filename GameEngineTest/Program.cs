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

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));
        
        var world = GameWorld.GetInstance();
        GameWorld.GlobalShader = new Shader(new[] {
                ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
                ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        });
        
        world.CreateMainCamera();
        
        world.AddProcessor(new MeshRenderer());

        
        var texture = new Texture(new Bitmap("./CheckerboardMap.png"));
        
        var testEntity = world.CreateEntity();
        var testTransform = testEntity.AddComponent(new Transform());
        testTransform.scale = new Vector3(0.5f);
        var mesh = testEntity.AddComponent(new Mesh(testTransform,true));
        mesh.SetVertices(
                new MeshVertex(new Vector3(-1f, 1f, 0f),Color.Red, new Vector2(0f, 1f)),
                new MeshVertex(new Vector3(1f, 1f, 0f),Color.Lime, new Vector2(1f, 1f)),
                new MeshVertex(new Vector3(1f, -1f, 0f),Color.Blue, new Vector2(1f, 0f)),
                new MeshVertex(new Vector3(-1f, -1f, 0f),Color.Blue, new Vector2(0f, 0f))
        );
        
        mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        mesh.SetTexture(texture);


        
        Game.GameLoopDraw += () =>
        {
            testTransform.rotation.X = (float)Glfw.Time * 2f;
            testTransform.rotation.Y = (float)Glfw.Time * 1.25f;
            testTransform.rotation.Z = (float)Glfw.Time;
            world.RunProcessors();
            
        };
        Game.Run();
    }
}