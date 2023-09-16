using System.Drawing;
using System.Drawing.Imaging;
using NLog;
using OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace OpenGLGameEngine.Core.Graphics;

public class Texture
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public readonly uint texureId;

    public static Texture defaultTexture = new Texture(new byte[] {
            255, 50, 255,
            50, 255, 50,
            0, 0,
            50, 255, 50,
            255, 50, 255,
            
    }, 2, 2, OpenGL.PixelFormat.Rgb);

    public Texture(Bitmap bitmap)
    {
        var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);
        texureId = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2d, texureId);
        Gl.TexImage2D(
            TextureTarget.Texture2d,
            0,
            InternalFormat.Rgba,
            data.Width, data.Height, 0,
            OpenGL.PixelFormat.Bgra, // using bgra here because somehow it is bgra in opengl when it is argb in C#
            PixelType.UnsignedByte, data.Scan0);
        Gl.GenerateMipmap(TextureTarget.Texture2d);
        bitmap.UnlockBits(data);
    }

    public Texture(byte[] bytes, int width, int height, OpenGL.PixelFormat pixelFormat)
    {
        texureId = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2d, texureId);
        Gl.TexImage2D(
            TextureTarget.Texture2d,
            0,
            InternalFormat.Rgba,
            width, height, 0,
            pixelFormat, // using bgra here because somehow it is bgra in opengl when it is argb in C#
            PixelType.UnsignedByte, bytes);
        Gl.GenerateMipmap(TextureTarget.Texture2d);
    }



    /// <summary>
    ///     Binds this texture for use in OpenGL
    ///     Using multiple textures in a single shader
    ///     <code>
    /// # In Glsl shader
    /// uniform sampler2D tex1
    /// uniform sampler2D tex2
    /// # In C#
    /// var a = new Texture(...)
    /// var b = new Texture(...)
    /// a.Bind(TextureUnit.Texture0)
    /// b.Bind(TextureUnit.Texture1)
    /// # a will be bound to tex1 and b will be bound to tex 2
    /// </code>
    ///     <param name="unit">The texture unit to bind this texture to. Useful when you want to use multiple textures in a shader program</param>
    /// </summary>
    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        Gl.ActiveTexture(unit);
        Gl.BindTexture(TextureTarget.Texture2d, texureId);
    }
}