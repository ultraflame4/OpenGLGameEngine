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
        
        Gl.TexParameteri(TextureTarget.Texture2d,TextureParameterName.TextureWrapS,Gl.MIRRORED_REPEAT);
        Gl.TexParameteri(TextureTarget.Texture2d,TextureParameterName.TextureWrapT,Gl.MIRRORED_REPEAT);
        Gl.TexParameteri(TextureTarget.Texture2d,TextureParameterName.TextureMinFilter,Gl.NEAREST);
        Gl.TexParameteri(TextureTarget.Texture2d,TextureParameterName.TextureMagFilter,Gl.LINEAR);


        Bitmap bitmap = new Bitmap("./CheckerboardMap.png");
        var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        uint texture = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2d, texture);
        Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba,
            data.Width, data.Height, 0, 
            OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
        Gl.GenerateMipmap(TextureTarget.Texture2d);
        bitmap.UnlockBits(data);
        
        
        float[] v1 = {
                // contains both position and color and texture
                -1f, 1f, 0f,    1f, 0f, 0f,     0f,1f,
                1f, 1f, 0f,     0f, 1f, 0f,     1f, 1f,
                1f, -1f, 0f,    0f, 0f, 1f,     1f, 0f,
                -1f, -1f, 0f,   0f, 0f, 1f,     0f, 0f
        };
        uint[] t1 = {
                0, 2, 1,
        };
        uint[] t2 = {
                0, 3, 2
        };

        var o = new VertexRenderObject(v1, 8, t1);
        o.SetVertexAttrib(0, 3, 0);
        o.SetVertexAttrib(1, 3, 3);
        o.SetVertexAttrib(2, 2, 6);
        
        var o2 = new VertexRenderObject(v1, 8, t2);
        o2.SetVertexAttrib(0, 3, 0);
        o2.SetVertexAttrib(1, 3, 3);
        o2.SetVertexAttrib(2, 2, 6);


        Game.GameLoopDraw += () =>
        {
            shader.Use();
            Gl.BindTexture(TextureTarget.Texture2d,texture);
            o.Draw();
            o2.Draw();
        };
        Game.Run();
    }
}