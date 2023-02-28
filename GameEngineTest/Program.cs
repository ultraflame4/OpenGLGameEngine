using NLog;
using OpenGL;
using OpenGLGameEngine;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Utils;
using System.Drawing;
using System.Numerics;
using GLFW;
using OpenGLGameEngine.Components;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Game;


namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));
        var world = GameWorld.GetInstance();
        world.AddProcessor(new MeshRenderer());

        
        world.CreateMainCamera();
        GameWorld.GlobalShader = new Shader(new[] {
                ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
                ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        });

        var texture = new Texture(new Bitmap("./CheckerboardMap.png"));



        var testEntity = world.CreateEntity();
        var comp = testEntity.AddComponent(new Transform());
        comp.scale = new Vector3(0.5f);
        
        var mesh = testEntity.AddComponent(new Mesh(comp,true));
        mesh.SetVertices(
                new MeshVertex(new Vector3(-1f, 1f, 0f),Color.Red, new Vector2(0f, 1f)),
                new MeshVertex(new Vector3(1f, 1f, 0f),Color.Lime, new Vector2(1f, 1f)),
                new MeshVertex(new Vector3(1f, -1f, 0f),Color.Blue, new Vector2(1f, 0f)),
                new MeshVertex(new Vector3(-1f, -1f, 0f),Color.Blue, new Vector2(0f, 0f))
        );
        mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        mesh.SetTexture(texture);
        
        // float[] v1 = {
        //         // contains both position and color and texture
        //         -1f, 1f, 0f, 1f, 0f, 0f, 0f, 1f,
        //         1f, 1f, 0f, 0f, 1f, 0f, 1f, 1f,
        //         1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f,
        //         -1f, -1f, 0f, 0f, 0f, 1f, 0f, 0f
        // };
        // uint[] t1 = {
        //         0, 2, 1, 0, 3, 2
        // };
        //
        // var o = new VertexRenderObject(Array.Empty<float>(), 8);
        // o.SetVertices(v1);
        // o.SetTriangles(t1);
        // o.SetVertexAttrib(0, 3, 0);
        // o.SetVertexAttrib(1, 3, 3);
        // o.SetVertexAttrib(2, 2, 6);


        int fov = 80;
        var proj = Matrix4x4.CreatePerspectiveFieldOfView((float)Utils.Deg2Rad(fov), WindowUtils.GetAspectRatio(), 0.01f, 100f);

        
        Game.GameLoopDraw += () =>
        {
            // todo in future move shaders and matrixs code to the camera component
            // shader.Use();
            comp.rotation.X = (float)Glfw.Time * 2f;
            comp.rotation.Y = (float)Glfw.Time * 1.25f;
            comp.rotation.Z = (float)Glfw.Time;
            // // var rotation = Matrix4x4.CreateRotationY((float)Glfw.Time) * Matrix4x4.CreateRotationX((float)Utils.Deg2Rad(45));
            // var transformMatrix =  comp.GetModelMatrix() * proj;
            // shader.SetUniform("transform", transformMatrix);
            //
            
            world.RunProcessors();
            
        };
        Game.Run();
    }
}