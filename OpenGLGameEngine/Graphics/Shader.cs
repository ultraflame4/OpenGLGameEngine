using OpenGL;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Graphics;

/// <summary>
/// This class simplifies the usage and creation of shaders and shaders programs
/// </summary>
public class Shader
{
    public readonly uint shaderProgram;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="shaders">Array of shaders loaded into memory. Use ShaderUtils to load them initially.</param>
    public Shader(uint[] shaders)
    {
        shaderProgram = ShaderUtils.CreateProgam(shaders);
    }

    /// <summary>
    /// Uses this shader program
    /// </summary>
    public void Use()
    {
        Gl.UseProgram(shaderProgram);
    }
}