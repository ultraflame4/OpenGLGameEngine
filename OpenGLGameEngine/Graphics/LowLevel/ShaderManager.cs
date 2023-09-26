using System.Reflection;
using OpenGL;
using OpenGLGameEngine.Core.Utils;

namespace OpenGLGameEngine.Graphics.LowLevel;

public class ShaderManager
{
    private static ShaderManager? _instance;

    public static ShaderManager instance =>
            _instance ?? throw new InvalidOperationException("ShaderManager not initialized!");

    public readonly SingleShader defaultVertexShader;
    public readonly SingleShader defaultFragmentShader;
    public readonly Shader defaultShader;
    public List<SingleShader> shaders = new();
    public List<Shader> programs = new();

    public ShaderManager()
    {
        defaultVertexShader = SingleFromResource("OpenGLGameEngine.Resources.Shaders.vertex.glsl");
        defaultFragmentShader = SingleFromResource("OpenGLGameEngine.Resources.Shaders.fragment.glsl");
        defaultShader = CreateProgram(defaultVertexShader, defaultFragmentShader);
    }

    /// <summary>
    /// Initializes the shader manager
    /// </summary>
    public static void Init() { _instance = new ShaderManager(); }

    public SingleShader CreateSingle(string source, ShaderType type)
    {
        var shader = new SingleShader() { id = ShaderUtils.CreateShader(type, source), type = type };
        shaders.Add(shader);
        return shader;
    }

    public SingleShader SingleFromFile(string filepath, ShaderType type)
    {
        var shader = new SingleShader() { id = ShaderUtils.LoadShaderFromPath(filepath, type), type = type };
        shaders.Add(shader);
        return shader;
    }
    
    public SingleShader SingleFromFile(string filepath, string suffix = ".glsl", ShaderType? type = null)
    {
        return SingleFromFile(filepath, type ?? GetTypeFromPath(filepath, suffix));
    }
    
    public SingleShader SingleFromResource(string resources_name, ShaderType type, Assembly? assembly = null)
    {
        var shader = new SingleShader() { id = ShaderUtils.LoadShaderFromResource(resources_name, type, assembly??Assembly.GetCallingAssembly()), type = type };
        shaders.Add(shader);
        return shader;
    }
    public SingleShader SingleFromResource(string filepath, string suffix = ".glsl", ShaderType? type = null, Assembly? assembly = null)
    {
        return SingleFromResource(filepath, type ?? GetTypeFromPath(filepath, suffix), assembly??Assembly.GetCallingAssembly());
    }
    
    public Shader CreateProgram(params SingleShader[] _shaders)
    {
        var program = new Shader(_shaders);
        programs.Add(program);
        return program;
    }
    public ShaderType GetTypeFromPath(string path, string suffix = ".glsl")
    {
        if (path.EndsWith("fragment" + suffix)) return ShaderType.FragmentShader;
        if (path.EndsWith("frag" + suffix)) return ShaderType.FragmentShader;
        if (path.EndsWith("vertex" + suffix)) return ShaderType.VertexShader;
        if (path.EndsWith("vert" + suffix)) return ShaderType.VertexShader;
        if (path.EndsWith("compute" + suffix)) return ShaderType.ComputeShader;
        if (path.EndsWith("geo" + suffix)) return ShaderType.GeometryShader;

        throw new InvalidOperationException("Could not determine shader type for path: " + path);
    }
}