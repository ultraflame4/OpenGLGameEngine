using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Components;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Core.Graphics;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Core.Windowing;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine;

public static class Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static void CreateMainWindow(string windowTitle, Keys? fullscreenKey = Keys.F11,
        WindowModes windowMode = WindowModes.Windowed, (int width, int height) windowSize = default)
    {
        GameWindow.Create(windowTitle, fullscreenKey, windowMode, windowSize);
    }

    private static void LoadDefaults()
    {
        
        if (MeshRenderer.defaultShader == null)
        {
            MeshRenderer.defaultShader = new Shader(new[] {
                    ShaderUtils.LoadShaderFromResource("OpenGLGameEngine.Resources.Shaders.vertex.glsl", ShaderType.VertexShader),
                    ShaderUtils.LoadShaderFromResource("OpenGLGameEngine.Resources.Shaders.fragment.glsl", ShaderType.FragmentShader)
            });
            logger.Info("Loaded default shaders as it was null!");
        }
    }

    public static void Start()
    {
        logger.Info("-----------------Loading Defaults----------------------");
        LoadDefaults();
        logger.Info("--------Loading Defaults END >>> Starting Game Loop----------------");
        WorldManager.CurrentWorld?.Start();
        GameWindow.GameLoopUpdate += () => { WorldManager.CurrentWorld?.TickUpdate();};
        GameWindow.GameLoopDraw += () => { WorldManager.CurrentWorld?.TickDraw(); };
        GameWindow.Run();
    }
}