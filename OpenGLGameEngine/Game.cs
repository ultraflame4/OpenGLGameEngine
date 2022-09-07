using System.Runtime.InteropServices;
using GLFW;
using NLog;
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
        LogManager.Configuration = Utils.GetNLogConfig();
        logger.Info($"Loaded logging configuration.");
        logger.Info($"GameEngine version: {Utils.VERSION}");
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
    public static void Create(string windowTitle)
    {
        // Start initialisation and create opengl context and window.
        logger.Info($"beginning initialisation and creation...");
        logger.Info($"Set Window Title: {windowTitle}");
        
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
        logger.Info("Creating window...");
        window = Glfw.CreateWindow(640, 480, windowTitle, GLFW.Monitor.None, Window.None);
        if (window == Window.None)
        {
            logger.Fatal("Window or OpenGL context failed");
            return;
        }
        Glfw.MakeContextCurrent(window);
        Import(Glfw.GetProcAddress);
        
    }


    public static void Run() { }

    private static void Stop()
    {
        Glfw.DestroyWindow(window);
        Glfw.Terminate();
    }


    private static void onGlfwError(ErrorCode errCode, IntPtr description_p)
    {
        string? description = Marshal.PtrToStringAnsi(description_p);
        logger.Error($"Glfw has encountered an error ({errCode}) {description_p}");
    }
}