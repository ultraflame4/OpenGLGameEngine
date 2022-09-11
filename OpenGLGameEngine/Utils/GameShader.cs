using NLog;
using OpenGL;

namespace OpenGLGameEngine.Utils;

/// <summary>
/// A utility class to help in things related to shaders and programs
/// </summary>
public static class GameShader
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Creates a shader of the specified type from the given source string.
    /// </summary>
    /// <param name="type">An OpenGL enum for the shader type.</param>
    /// <param name="source">The source code of the shader.</param>
    /// <returns>The created shader. No error checking is performed for this basic example.</returns>
    /// <remarks>Taken from https://gist.github.com/dcronqvist/4e83dc3a4defe5780f1d4b6cac7558f6/#file-hellotriangle-cs-L134</remarks>
    public static uint CreateShader(ShaderType type, string source)
    {
        var shader = Gl.CreateShader(type);
        Gl.ShaderSource(shader, source.Split("\n"));
        Gl.CompileShader(shader);
        return shader;
    }

    public static uint LoadShaderFromPath(string path, ShaderType type)
    {
        logger.Info($"Attempting to load shader from path {path}");
        using (StreamReader file = new StreamReader(path))
        {
            return CreateShader(type, file.ReadToEnd());
        }
    }

    public static void LoadShaderFromResource(string path,int shader_type)
    {
        logger.Info($"Attempting to load shader from path {path}");
        //todo
    }
}