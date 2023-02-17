using System.Drawing;
using System.Drawing.Imaging;
using NLog;
using OpenGL;
using PixelFormat = OpenGL.PixelFormat;

namespace OpenGLGameEngine.Graphics;

public class Texture
{
    static Logger logger = LogManager.GetCurrentClassLogger();
    public readonly uint texureId;
    /// <summary>
    /// Sets the opengl configurations for textures
    /// </summary>
    public static void ConfigureOpenGl()
    {
        logger.Info("Configuring OpenGl Textures...");
        Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, Gl.MIRRORED_REPEAT);
        Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, Gl.MIRRORED_REPEAT);
        Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, Gl.NEAREST);
        Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, Gl.LINEAR);
    }

    public Texture(Bitmap bitmap)
    {
        var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        texureId = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2d, texureId);
        Gl.TexImage2D(
            TextureTarget.Texture2d, 
            0,
            InternalFormat.Rgba,
            data.Width, data.Height, 0,
            PixelFormat.Bgra, // using bgra here because somehow it is bgra in opengl when it is argb in C#
            PixelType.UnsignedByte, data.Scan0);
        Gl.GenerateMipmap(TextureTarget.Texture2d);
        bitmap.UnlockBits(data);
        
    }

    /// <summary>
    /// Binds this texture for use in OpenGL
    /// Using multiple textures in a single shader
    /// <code>
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
    /// <param name="unit">The texture unit to bind this texture to. Useful when you want to use multiple textures in a shader program</param>
    /// </summary>
    public void Bind(TextureUnit unit=TextureUnit.Texture0)
    {
        Gl.ActiveTexture(unit);
        Gl.BindTexture(TextureTarget.Texture2d, texureId);
    }
    
}