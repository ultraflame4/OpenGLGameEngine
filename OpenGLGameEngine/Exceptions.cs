using System.Runtime.Serialization;
using System.Text;
using OpenGL;

namespace OpenGLGameEngine;

public class GameEngineException : Exception
{
    public GameEngineException() { }
    protected GameEngineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public GameEngineException(string? message) : base(message) { }
    public GameEngineException(string? message, Exception? innerException) : base(message, innerException) { }
}

public class ShaderCompilationException : GameEngineException
{
    public ShaderType shaderType;

    public ShaderCompilationException(string? message, ShaderType shaderType, StringBuilder infoLog) : base(message)
    {
        this.shaderType = shaderType;
        Data.Add("ShaderType", shaderType.ToString());
        Data.Add("InfoLog", $"\n{infoLog}");
    }
}