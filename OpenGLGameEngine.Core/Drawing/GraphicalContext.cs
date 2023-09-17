using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Windowing;
using ErrorCode = OpenGL.ErrorCode;

namespace OpenGLGameEngine.Core.Drawing;

/// <summary>
/// Represents a graphical context which includes and manages the following:
///
/// Shader drawing,
/// Shader uniform blocks,
/// Current camera matrices,
/// OpenGl state, and initialising OpenGL.
///
/// Other things that related to opengl drawing.
///
/// Note that this class does not create a window or a opengl context.
/// </summary>
public class GraphicalContext
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    protected static void SetWindowHints()
    {
        // Set some common hints for the OpenGL profile creation
        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        // Set opengl version
        Glfw.WindowHint(Hint.ContextVersionMajor, 4);
        Glfw.WindowHint(Hint.ContextVersionMinor, 6);

        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.Decorated, true);
        Glfw.WindowHint(Hint.AutoIconify, false);
        Glfw.WindowHint(Hint.Samples, 4);
    }
    
    /// <summary>
    /// Configures the graphical context. This should be called after creating the window and opengl context.
    /// </summary>
    protected void Configure()
    {
        logger.Debug("Configuring OpenGL settings...");
        Gl.Enable(EnableCap.DepthTest);
        Gl.Enable(EnableCap.Multisample);
        logger.Trace("Configuring common texture settings...");
        Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, Gl.MIRRORED_REPEAT);
        Gl.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, Gl.MIRRORED_REPEAT);
        CheckError();
    }

    /// <summary>
    /// Checks for any OpenGL errors and logs them.
    /// </summary>
    public void CheckError()
    {
        ErrorCode code;
        while ((code = Gl.GetError()) != ErrorCode.NoError) logger.Error("OpenGL Error Code: {code} !", code);
    }

    /// <summary>
    /// Clears the buffers for drawing. Shorthand for <see cref="Gl.Clear"/>
    /// </summary>
    public void Clear() { Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); }
}