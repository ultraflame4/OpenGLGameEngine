using System.Numerics;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Utils;

namespace OpenGLGameEngine.Core.Drawing;

/// <summary>
///     This class simplifies the usage and creation of shaders and shaders programs
/// </summary>
public class Shader
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public readonly uint shaderProgram;

    /// <summary>
    /// </summary>
    /// <param name="shaders">Array of shaders loaded into memory. Use ShaderUtils to load them initially.</param>
    public Shader(uint[] shaders)
    {
        shaderProgram = ShaderUtils.CreateProgam(shaders);
    }

    /// <summary>
    ///     Uses this shader program
    /// </summary>
    public void Use()
    {
        Gl.UseProgram(shaderProgram);
    }

    public int GetUniformLocation(string name)
    {
        var uniformLocation = Gl.GetUniformLocation(shaderProgram, name);
        if (uniformLocation == -1)
        {
            logger.Error($"Could not get uniform location for name: {name} from shaderProgram {shaderProgram}!");
            return -1;
        }
        return uniformLocation;
    }
    

    public void SetUniform(string name, bool value)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        Gl.Uniform1i(uniformLocation, 1, value);
    }

    public void SetUniform(string name, int value)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        Gl.Uniform1i(uniformLocation, 1, value);
    }

    public void SetUniform(string name, float value)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        Gl.Uniform1f(uniformLocation, 1, value);
    }

    public void SetUniform(string name, Matrix4x4 value)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        Gl.UniformMatrix4f(uniformLocation, 1, false, value);
    }
}