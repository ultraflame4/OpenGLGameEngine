
using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Utils;
using Window = OpenGLGameEngine.Core.Windowing.Window;

namespace OpenGLGameEngine;

public static class Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public static GameWorld? CurrentWorld { get; private set; }

    public static Window MainWindow;

    public static void CreateMainWindow(string windowTitle, Keys? fullscreenKey = Keys.F11, WindowModes windowMode = WindowModes.Windowed, (int width, int height) windowSize = default)
    {
        MainWindow = new Window(windowTitle,fullscreenKey,windowMode,windowSize);
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
                    ShaderUtils.LoadShaderFromResource("OpenGLGameEngine.Resources.Shaders.fragment.glsl", ShaderType.FragmentShader)
            });
            logger.Info("Loaded default shaders into GlobalShader as it was null!");
        }
    }

    public static void Start()
    {       
        Gl.Initialize();
        logger.Info("-----------------Start Window Creation----------------------");
        MainWindow.Start();
        
        MainWindow.DrawThreadStart += () =>
        {
            logger.Info("-----------------Loading Defaults----------------------");
            LoadDefaults();
        };
        

        
        logger.Info("--------Loading Defaults END >>> Starting Game Loop----------------");
        MainWindow.DrawEvent += () =>
        {
            CurrentWorld?.MeshRenderer.processComponents();
            CurrentWorld?.EntityScriptExecutor.ProcessDraws();
        };
        
        // CurrentWorld?.EntityScriptExecutor.ProcessStarts();
        
        // GameWindow.GameLoopUpdate += () =>
        // {
        //     CurrentWorld?.RunProcessors();
        //     CurrentWorld?.EntityScriptExecutor.ProcessUpdates();
        // };
        // GameWindow.GameLoopDraw += () =>
        // {
        //     CurrentWorld?.MeshRenderer.processComponents();
        //     CurrentWorld?.EntityScriptExecutor.ProcessDraws();
        // };
        // GameWindow.Run();
        MainWindow.Run();
    }
}