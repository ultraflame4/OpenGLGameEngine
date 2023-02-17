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
    /// Binds this texture for use in OpenGl
    /// </summary>
    public void Bind()
    {
        Gl.BindTexture(TextureTarget.Texture2d, texureId);
    }
    
}