using NLog;
using OpenGL;
using OpenGLGameEngine;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Utils;
using System.Drawing;

using System.Numerics;
using GLFW;


namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed,windowSize:(720,720));


        Shader shader = new Shader(new[] {
                ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
                ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        });

        var texture = new Texture(new Bitmap("./CheckerboardMap.png"));


        float[] v1 = {
                // contains both position and color and texture
                -1f, 1f, 0f, 1f, 0f, 0f, 0f, 1f,
                1f, 1f, 0f, 0f, 1f, 0f, 1f, 1f,
                1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f,
                -1f, -1f, 0f, 0f, 0f, 1f, 0f, 0f
        };
        uint[] t1 = {
                0, 2, 1, 0, 3, 2
        };
        
        var o = new VertexRenderObject(v1, 8, t1);
        o.SetVertexAttrib(0, 3, 0);
        o.SetVertexAttrib(1, 3, 3);
        o.SetVertexAttrib(2, 2, 6);

        int fov = 90;
        var proj = Matrix4x4.CreatePerspectiveFieldOfView((float)Utils.Deg2Rad(fov), WindowUtils.GetAspectRatio(), 0.01f, 100f);

        Game.GameLoopDraw += () =>
        {
            shader.Use();
            var rotation = Matrix4x4.CreateRotationY((float)Glfw.Time)*Matrix4x4.CreateRotationX((float)Utils.Deg2Rad(45));
            var transformMatrix = rotation * Matrix4x4.CreateTranslation(new Vector3(0.5f,0,-1f)) * Matrix4x4.CreateScale(0.5f) * proj;
            shader.SetUniform("transform",transformMatrix);

            texture.Bind();
            o.Draw();
        };
        Game.Run();
    }
}