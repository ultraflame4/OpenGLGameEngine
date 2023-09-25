using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Core.Windowing;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;
using Window = OpenGLGameEngine.Core.Windowing.Window;

namespace OpenGLGameEngine;

public static class Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private static Window? _mainWindow = null;

    public static Window MainWindow => _mainWindow ?? throw new InvalidOperationException("Call Game.Init() first!");


    public static void Init(string windowTitle, Keys? fullscreenKey = Keys.F11,
        WindowModes windowMode = WindowModes.Windowed, (int width, int height) windowSize = default)
    {
        WindowManager.Init();
        _mainWindow = Window.Create(windowTitle, new WindowRect(0,0, windowSize.width,windowSize.height), windowMode, true);
        _mainWindow.CreateGLContext();
        _mainWindow.InitInput();
        RenderPipeline.Init(_mainWindow);

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

        while (!MainWindow.shouldClose)
        {
            GameTime.UpdateDeltaTime(Glfw.Time);
            MainWindow.Poll();
            WorldManager.CurrentWorld?.TickUpdate();
            WorldManager.CurrentWorld?.TickDraw();
            RenderPipeline.Render();
            MainWindow.SwapBuffers();
        }
        Stop();
    }

    public static void Stop()
    {
        logger.Info("Exited game loop!");
        WindowManager.Terminate();
        logger.Info("Game shutdown and exited successfully.");
    }
}