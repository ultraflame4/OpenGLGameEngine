using OpenGL;

namespace OpenGLGameEngine.Graphics.LowLevel;

/// <summary>
/// A single shader loaded in openGL
/// </summary>
public struct SingleShader
{
    public uint id { get; init; }
    public ShaderType type { get; init; }
}