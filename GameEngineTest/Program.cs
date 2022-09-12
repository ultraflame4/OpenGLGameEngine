using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine;
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
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f,  0.5f, 0.0f
        };
        uint vbo = Gl.GenBuffer();
        uint vao = Gl.GenVertexArray();
        
        Gl.BindVertexArray(vao); // set this vao as current Bound VertexArray so we can safe the "config"
        
        // Do da "config" for this vbo
        Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        Gl.BufferData(BufferTarget.ArrayBuffer,(uint)(sizeof(float) * vertices.Length),vertices,BufferUsage.StaticDraw);
        
        Gl.VertexAttribPointer(0,3,VertexAttribType.Float,false,3*sizeof(float),IntPtr.Zero);
        Gl.EnableVertexAttribArray(0);

        Game.GameLoopDraw += () =>
        {
            Gl.UseProgram(shaderProgram);
            Gl.BindVertexArray(vao);
            Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
        };
        Game.Run();
    }
}