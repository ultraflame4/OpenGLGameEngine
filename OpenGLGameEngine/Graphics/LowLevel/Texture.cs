﻿using OpenGL;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Math;
using StbImageSharp;

namespace OpenGLGameEngine.Graphics.LowLevel;

public struct TextureConfig
{
    public InternalFormat internalFormat = InternalFormat.Rgba;
    public TextureFilterType minFilter = TextureFilterType.NEAREST;
    public TextureFilterType magFilter = TextureFilterType.LINEAR;


    public TextureTarget textureTarget => TextureTarget.Texture2d;
    public TextureConfig() { }
}

public class Texture : IGLObj
{
    public static Texture defaultTexture = Texture.FromBytes(new byte[] {
            255, 50, 255, 50, 255, 50,
            0, 0,
            50, 255, 50, 255, 50, 255
    }, 2, 2, OpenGL.PixelFormat.Rgb, new() {
            minFilter = TextureFilterType.NEAREST,
            magFilter = TextureFilterType.NEAREST
    });

    public uint id { get; }
    public int width { get; private set; }
    public int height { get; private set; }
    protected readonly TextureConfig config;

    /// <summary>
    /// Tells OpenGl to create a new texture. This also binds the texture.
    /// 
    /// </summary>
    /// <param name="config"></param>
    protected Texture(TextureConfig config, int height, int width)
    {
        this.config = config;
        this.height = height;
        this.width = width;
        id = Gl.GenTexture();
        Gl.BindTexture(config.textureTarget, id);
        Gl.TexParameteri(config.textureTarget, TextureParameterName.TextureMinFilter, this.config.minFilter);
        Gl.TexParameteri(config.textureTarget, TextureParameterName.TextureMagFilter, this.config.magFilter);
    }

    protected void GenerateMipmap() { Gl.GenerateMipmap(config.textureTarget); }

    public static Texture FromBytes(byte[] bytes, int width, int height, OpenGL.PixelFormat pixelFormat,
        TextureConfig config)
    {
        Texture tex = new Texture(config, width, height);
        Gl.TexImage2D(
            config.textureTarget,
            0,
            config.internalFormat,
            width, height, 0,
            pixelFormat,
            PixelType.UnsignedByte, bytes);
        tex.GenerateMipmap();
        return tex;
    }

    public static Texture FromFile(string path, TextureConfig config)
    {
        using var fs = File.OpenRead(path);
        var result = ImageResult.FromStream(fs);
        Texture tex = new Texture(config, result.Height, result.Width);
        Gl.TexImage2D(
            config.textureTarget,
            0,
            config.internalFormat,
            result.Width, result.Height, 0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte, result.Data);
        return tex;
    }

    // public static Texture FromBitmap(Bitmap bitmap, TextureConfig config)
    // {
    //     
    //     var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly,
    //         PixelFormat.Format32bppArgb);
    //     Texture tex = new Texture(config,data.Width,data.Height);
    //     Gl.TexImage2D(
    //         config.textureTarget,
    //         0,
    //         config.internalFormat,
    //         data.Width, data.Height, 0,
    //         OpenGL.PixelFormat.Bgra, // using bgra here because somehow it is bgra in opengl when it is argb in C#
    //         PixelType.UnsignedByte, data.Scan0);
    //     tex.GenerateMipmap();
    //     bitmap.UnlockBits(data);
    //     return tex;
    // }

    /// <summary>
    /// An empty texture with the specified width and height and config. Good for creating render targets.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static Texture CreateEmpty(int width, int height, TextureConfig config)
    {
        Texture tex = new Texture(config, width, height);
        WindowUtils.CheckError();
        Gl.TexImage2D(config.textureTarget, 0, config.internalFormat, width, height, 0, OpenGL.PixelFormat.Rgba,
            PixelType.UnsignedByte, IntPtr.Zero);
        tex.GenerateMipmap();
        return tex;
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
        Gl.BindTexture(config.textureTarget, id);
    }

    /// <summary>
    /// Resizes the texture to the specified size. This will clear the texture.
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Resize(Point size)
    {
        
        width = size.X;
        height = size.Y;
        Bind();
        Gl.TexImage2D(config.textureTarget, 0, config.internalFormat, width, height, 0, OpenGL.PixelFormat.Rgba,
            PixelType.UnsignedByte, IntPtr.Zero);
        GenerateMipmap();
    }
}