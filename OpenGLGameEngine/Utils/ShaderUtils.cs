using System.Text;
using NLog;
using OpenGL;

namespace OpenGLGameEngine.Utils;

/// <summary>
/// A utility class to help in things related to shaders and programs
/// </summary>
public static class ShaderUtils
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
        string[] sourceLines = source.Split("\n");
        // Add \n back for each line because split will remove it. If the array does not have newlines, this opengl binding throws a fit! . _ .
        for (var i = 0; i < sourceLines.Length; i++)
        {
            sourceLines[i] += "\n";
        }
        
        Gl.ShaderSource(shader, sourceLines);

        Gl.CompileShader(shader);
        Gl.GetShader(shader, ShaderParameterName.CompileStatus, out var success);
        
        if (success != 1)
        {
            StringBuilder infoLog = new StringBuilder(1024);
            Gl.GetShaderInfoLog(shader, 1024, out int a, infoLog);
            logger.Error(new ShaderCompilationException($"Shader Compilation failed for shader {shader}!", type, infoLog), "Error while compiling shader!\n");
        }

        logger.Debug($"Created shader with reference index {shader} of type {type}");
        return shader;
    }

    /// <summary>
    /// Loads, Compiles and Create a shader from a file.
    /// <b>note : for relative paths, it will be relative to the location of your program's exe file.</b>
    /// </summary>
    /// <param name="path">The filepath of the file</param>
    /// <param name="type">The opengl shader type.</param>
    /// <returns></returns>
    public static uint LoadShaderFromPath(string path, ShaderType type)
    {
        string fullPath = Path.GetFullPath(path);
        logger.Info($"Loading shader from path {fullPath}");
        using (StreamReader file = new StreamReader(path))
        {
            return CreateShader(type, file.ReadToEnd());
        }
    }

    /// <summary>
    /// Loads a shader from a resource
    /// </summary>
    /// <param name="path">The resource path</param>
    /// <param name="type">Shader type.</param>
    public static void LoadShaderFromResource(string path, ShaderType type)
    {
        logger.Info($"Loading shader from resource {Path.GetFullPath(path)}");
        //todo
    }

    /// <summary>
    /// A utility method to create a shader program for opengl. This will also link shaders that are attached,
    /// </summary>
    /// <param name="shaders">The shaders you want to attach to the shader program</param>
    /// <returns>The shader program</returns>
    public static uint CreateProgam(uint[]? shaders)
    {
        uint program = Gl.CreateProgram();
        if (shaders is not null)
        {
            foreach (uint shader in shaders)
            {
                Gl.AttachShader(program, shader);
                logger.Debug($"CreateProgram - Attached shader {shader} to program {program}");
            }
        }

        Gl.LinkProgram(program);
        return program;
    }
}