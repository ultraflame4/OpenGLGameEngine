using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine;

public static class Game
{
    public static GameWorld? CurrentWorld { get; private set; }
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void CreateMainWindow(string windowTitle, Keys? fullscreenKey = Keys.F11, WindowModes windowMode = WindowModes.Windowed, (int width, int height) windowSize = default)
    {
        GameWindow.Create(windowTitle, fullscreenKey, windowMode, windowSize);
    }

    public static GameWorld GetCurrentWorld()
    {
        if (CurrentWorld == null) CurrentWorld = new GameWorld();
        return CurrentWorld;
    }

    private static void LoadDefaults()
    {
        if (GameWorld.GlobalShader == null)
        {
            GameWorld.GlobalShader = new Shader(new[] {
                    ShaderUtils.LoadShaderFromResource("OpenGLGameEngine.Resources.Shaders.vertex.glsl", ShaderType.VertexShader),
                    ShaderUtils.LoadShaderFromPath("OpenGLGameEngine.Resources.Shaders.fragment.glsl", ShaderType.FragmentShader)
            });
            logger.Info("Loaded default shaders into GlobalShader as it was null!");
        }

    }


    public static void Start()
    {
        LoadDefaults();
        GameWindow.Run();
    }
}