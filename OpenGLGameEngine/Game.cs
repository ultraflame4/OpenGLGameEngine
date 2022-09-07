using System.Runtime.InteropServices;
using GLFW;
using NLog;
using OpenGLGameEngine.Utils;
using static OpenGL.GL;
using Monitor = System.Threading.Monitor;

namespace OpenGLGameEngine;

/// <summary>
/// The main class.
/// <br/>
/// <br/>
/// * portions of the code were modified and copied from https://gist.github.com/dcronqvist/4e83dc3a4defe5780f1d4b6cac7558f6
/// </summary>
public static class Game
{
    static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    static Window window;
    
    static Game()
    {
        LogManager.Configuration = Utils.Utils.GetNLogConfig();
        logger.Info($"Loaded logging configuration.");
        logger.Info($"GameEngine version: {Utils.Utils.VERSION}");
    }

    
    private static void PrepareContext()
    {
        // Set some common hints for the OpenGL profile creation
        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.Decorated, true);
    }
    public static void Create(
        string windowTitle,
        WindowModes windowMode=WindowModes.Windowed,
        (int width,int height) windowSize = default
        )
    {
        if (windowSize.Equals(default))
        {
            windowSize = WindowUtils.DefaultWindowSize;
        }
        
        
        // Start initialisation and create opengl context and window.
        logger.Info($"beginning initialisation and creation...");
        
        logger.Debug($"Finding glfw dll at {Directory.GetCurrentDirectory()}...");
        if (!Glfw.Init())
        {
            logger.Fatal("Glfw Failed to initialised!");
            return;
        }
        logger.Info("Glfw initialised successfully!");
        Glfw.SetErrorCallback(onGlfwError);
        
        // Window and context creation
        PrepareContext();
        logger.Info("Begin window and context creation...");
        logger.Info($"- Set Window Title: {windowTitle}");
        logger.Info($"- Set Window Mode: {windowMode.ToString()}");
        logger.Info($"- Set Window Size: {windowMode.ToString()}");

        window = Glfw.CreateWindow(windowSize.width, windowSize.height, windowTitle, GLFW.Monitor.None, Window.None);
        WindowUtils.UpdateWindowSpacialData(window);
        if (window == Window.None)
        {
            logger.Fatal("Window or OpenGL context failed");
            return;
        }
        //todo Add Mode fullscreen to windowed mode switching
        switch (windowMode)
        {
            case WindowModes.Windowed:
                // do nothing cuz it starts in windowed mode
                break;
            case WindowModes.Maximised:
                Glfw.MaximizeWindow(window);
                break;
            case WindowModes.Fullscreen:
                WindowUtils.SetWindowFullscreen(true,window);
                break;
            case WindowModes.Borderless:
                //todo
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(windowMode), windowMode, null);
        }
        Glfw.MakeContextCurrent(window);
        Import(Glfw.GetProcAddress);
        logger.Info("Create window success!");

    }


    public static void Run()
    {
        while (!Glfw.WindowShouldClose(window))
        {
            Glfw.SwapBuffers(window);
            Glfw.PollEvents();
            glClear(GL_COLOR_BUFFER_BIT);
            Update();
            Draw();
        }
        Stop();
    }

    private static void Draw()
    {
        
    }
    private static void Update()
    {
        
    }
    
    private static void Stop()
    {
        Glfw.Terminate();
    }


    private static void onGlfwError(ErrorCode errCode, IntPtr description_p)
    {
        string? description = Marshal.PtrToStringAnsi(description_p);
        logger.Error($"Glfw has encountered an error ({errCode}) {description_p}");
    }
}