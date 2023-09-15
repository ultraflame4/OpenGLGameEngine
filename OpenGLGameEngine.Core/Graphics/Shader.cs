using System.Numerics;
using System.Runtime.CompilerServices;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Utils;

namespace OpenGLGameEngine.Core.Graphics;

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
            logger.Error($"Could not get uniform location for name: {name} from shaderProgram {shaderProgram}");
            return -1;
        }

        return uniformLocation;
    }

    /// <summary>
    ///     Tells opengl to use this shader if not already in use <br />
    ///     Use this where you expect the shader program to already be in use.
    /// </summary>
    private void AutoUse(string callerFile, int lineno)
    {
        Gl.GetInteger(GetPName.CurrentProgram, out int current_shader);
        if (current_shader != shaderProgram)
        {
            logger.Warn($"{callerFile}({lineno}) !! Shader Program ({shaderProgram}) is not in use! Will auto use!! (Current shader: {current_shader})");
            Use();
        }
    }

    public void AutoUse([CallerLineNumber] int lineno = -1, [CallerFilePath] string callerFile = "")
    {
        AutoUse(callerFile, lineno);
    }

    public void SetUniform(string name, bool value, [CallerFilePath] string callerFile = "", [CallerLineNumber] int lineno = -1)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        AutoUse(callerFile, lineno);
        Gl.Uniform1i(uniformLocation, 1, value);
    }

    public void SetUniform(string name, int value, [CallerFilePath] string callerFile = "", [CallerLineNumber] int lineno = -1)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        AutoUse(callerFile, lineno);
        Gl.Uniform1i(uniformLocation, 1, value);
    }

    public void SetUniform(string name, float value, [CallerFilePath] string callerFile = "", [CallerLineNumber] int lineno = -1)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        AutoUse(callerFile, lineno);
        Gl.Uniform1f(uniformLocation, 1, value);
    }

    public void SetUniform(string name, Matrix4x4 value, [CallerFilePath] string callerFile = "", [CallerLineNumber] int lineno = -1)
    {
        var uniformLocation = GetUniformLocation(name);
        if (uniformLocation == -1) return;
        AutoUse(callerFile, lineno);
        Gl.UniformMatrix4f(uniformLocation, 1, false, value);
    }
}