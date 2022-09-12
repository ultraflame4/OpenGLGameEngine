using System.Runtime.InteropServices;
using GLFW;
using NLog;
using OpenGLGameEngine.Utils;

using ErrorCode = GLFW.ErrorCode;
using Monitor = GLFW.Monitor;
using OpenGL;
namespace OpenGLGameEngine;

/// <summary>
/// The class containing the game loop and creation of window.
/// <br/>
/// <br/>
/// * portions of the code were modified and copied from https://gist.github.com/dcronqvist/4e83dc3a4defe5780f1d4b6cac7558f6
/// </summary>
public static class Game
{
    static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    static Window window;

    /// <summary>
    /// This event is invoked when the game loops exits and the window closes.
    /// </summary>
    public static event Action GameLoopExit;
    public static event Action GameLoopDraw;
    public static event Action GameLoopUpdate;

    static Game()
    {
        Utils.Utils.ConfigureNLog(logger);
        logger.Info($"GameEngine version: {Utils.Utils.VERSION}");
    }


    /// <summary>
    /// Initialises the game engine and creates the window
    /// </summary>
    /// <param name="windowTitle">The title of the window</param>
    /// <param name="fullscreenKey">The key to toggle fullscreen when pressed. Set to null to disable</param>
    /// <param name="windowMode">The display mode: windowed, maximised, fullscreen, fullscreen borderless.</param>
    /// <param name="windowSize">Size of the window in windowed mode.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Create(
        string windowTitle,
        Nullable<Keys> fullscreenKey = Keys.F11,
        WindowModes windowMode = WindowModes.Windowed,
        (int width, int height) windowSize = default
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
        logger.Info($"GLFW Version: {Glfw.VersionString}");

        logger.Info("GLFW detected Monitors available:");
        for (var i = 0; i < Glfw.Monitors.Length; i++)
        {
            Monitor m = Glfw.Monitors[i];
            logger.Info($"- index {i} Resolution: {m.WorkArea.Width}x{m.WorkArea.Height} position {m.WorkArea.X},{m.WorkArea.Y}");
        }

        // Window and context creation
        WindowUtils.SetWindowHints();
        logger.Info("Begin window and context creation...");
        logger.Info($"- Set Window Title: {windowTitle}");
        logger.Info($"- Set Window Mode: {windowMode.ToString()}");
        logger.Info($"- Set Window Size: {windowMode.ToString()}");

        window = Glfw.CreateWindow(windowSize.width, windowSize.height, windowTitle, Monitor.None, Window.None);
        
        if (window == Window.None)
        {
            logger.Fatal("Window or OpenGL context failed");
            return;
        }
        
        WindowUtils.SetWindowDisplayMode(window, windowMode);
        Gl.Initialize();
        Glfw.MakeContextCurrent(window);
        Glfw.WindowHint(Hint.OpenglDebugContext,true);
        Gl.DebugMessageCallback(onOpenGlError,0);
        logger.Info("OpenGL Context created successfully. !");
        logger.Info("OpenGL configuration:");
        logger.Info($"- Version: {Gl.GetString(StringName.Version)}");
        logger.Info($"- ShaderLang Version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
        logger.Info($"- Renderer: {Gl.GetString(StringName.Renderer)}");
        logger.Info($"- Vender: {Gl.GetString(StringName.Vendor)}");

        logger.Info("Create window success!");
        WindowUtils.UpdateWindowSpacialData(window); // must be called so that the user's window size config is not changed
        
        logger.Info("Configuring and initiating keyboard input");
        KeyboardMouseInput.Init(window);

        logger.Info($"Set toggle fullscreen key: {fullscreenKey}");

        KeyboardMouseInput.OnKeyDown += (key, code, state, mods) =>
        {
            int winX, winY;
            Glfw.GetWindowPosition(window, out winX, out winY);
            if (key == fullscreenKey)
            {
                logger.Info($"Toggling fullscreen!");
                WindowUtils.ToggleFullscreen(window);
            }
        };
        
        Glfw.SwapInterval(1);
        logger.Info("!!!!!!!!!!!!!!!!!!!!! Initial configuration and initialisation done !!!!!!!!!!!!!!!!!!!!!");
    }



    public static void Run()
    {
        while (!Glfw.WindowShouldClose(window))
        {
            Glfw.SwapBuffers(window);
            
            GameTimer._deltatime = (float)(Glfw.Time - GameTimer._lastTotalElapsed);
            GameTimer._lastTotalElapsed = Glfw.Time;
            
            Glfw.PollEvents();
            
            Update();

            int width, height;
            Glfw.GetFramebufferSize(window, out width, out height);

            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            
            Draw();
            
        }

        Stop();
    }

    private static void Draw()
    {

        // draw here
        GameLoopDraw?.Invoke();
    }

    private static void Update()
    {
        KeyboardMouseInput.Update();
        GameLoopUpdate?.Invoke();
    }

    private static void Stop()
    {
        logger.Info("Exited game loop!");
        logger.Debug("Invoking GameLoopExit events...");
        GameLoopExit?.Invoke();
        logger.Debug("Event invocation success. Terminating Glfw...");
        Glfw.Terminate();
        logger.Debug("Terminated Glfw !");
        logger.Info("Game shutdown and exited successfully.");
    }

    private static void onGlfwError(ErrorCode errCode, IntPtr description_p)
    {
        string? description = Marshal.PtrToStringAnsi(description_p);
        logger.Error($"Glfw has encountered an error ({errCode}) {description_p}");
    }
    
    private static void onOpenGlError(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
    {
        string? msg = Marshal.PtrToStringAnsi(message);
        
        logger.Warn($"OPENGL (id {id}) | DebugSource.{source} | DebugType.{type} | {severity} | {message}");
    }

}