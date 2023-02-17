using NLog;
using OpenGL;
using OpenGLGameEngine;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Utils;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed);


        Shader shader = new Shader(new[] {
                ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
                ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        });
        shader.SetUniform("test",1f);
        
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

        Game.GameLoopDraw += () =>
        {
            shader.Use();

            texture.Bind();
            o.Draw();
        };
        Game.Run();
    }
}