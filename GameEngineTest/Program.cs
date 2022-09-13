using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Inputs;
using OpenGLGameEngine.Utils;

namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed);
        uint vertexShader = ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader);
        uint fragmentShader = ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader);
        uint shaderProgram = ShaderUtils.CreateProgam(new[] { vertexShader, fragmentShader });

        float[] vertices = {
                -1f, 1f, 0f, 1f, 0f, 0f,
                1f, 1f, 0f, 0f, 1f, 0f,
                1f, -1f, 0f, 0f, 0f, 1f,
                -1f, -1f, 0.0f, 0f, 0f, 1f
        };
        uint[] triangles = {
                0, 2, 1,
                0, 3, 2
        };


        var o = new VertexRenderObject(vertices, 6, triangles);
        o.SetVertexAttrib(0, 3, 0);
        o.SetVertexAttrib(1, 3, 3);


        Game.GameLoopDraw += () =>
        {
            Gl.UseProgram(shaderProgram);
            o.Draw();
        };
        Game.Run();
    }
}