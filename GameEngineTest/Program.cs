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


namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));


        Shader shader = new Shader(new[] {
                ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
                ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        });

        var texture = new Texture(new Bitmap("./CheckerboardMap.png"));

        var testEntity = World.GetInstance().createEntity();
        var comp = testEntity.AddComponent(new Transform());
        comp.scale = new Vector3(0.5f);
        
        var mesh = testEntity.AddComponent(new Mesh(true));
        mesh.SetVertices(
                new MeshVertex(new Vector3(-1f, 1f, 0f),Color.Red, new Vector2(0f, 1f)),
                new MeshVertex(new Vector3(1f, 1f, 0f),Color.Lime, new Vector2(1f, 1f)),
                new MeshVertex(new Vector3(1f, -1f, 0f),Color.Blue, new Vector2(1f, 0f)),
                new MeshVertex(new Vector3(-1f, -1f, 0f),Color.Blue, new Vector2(0f, 0f))
        );
        mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        
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
            shader.Use();
            comp.rotation.X = (float)Glfw.Time * 2f;
            comp.rotation.Y = (float)Glfw.Time * 1.25f;
            comp.rotation.Z = (float)Glfw.Time;
            // var rotation = Matrix4x4.CreateRotationY((float)Glfw.Time) * Matrix4x4.CreateRotationX((float)Utils.Deg2Rad(45));
            var transformMatrix =  comp.GetModelMatrix() * proj;
            shader.SetUniform("transform", transformMatrix);

            // todo In the future, texture.Bind and mesh.Draw should be called by their respective systems in the ECS framework.
            texture.Bind(); 
            // o.Draw();
            mesh.Draw();
            
        };
        Game.Run();
    }
}