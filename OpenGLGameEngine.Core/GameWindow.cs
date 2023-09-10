using System.Numerics;
using System.Runtime.InteropServices;
using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Graphics;
using OpenGLGameEngine.Core.Windowing;
using OpenGLGameEngine.Utils;
using ErrorCode = GLFW.ErrorCode;
using Window = OpenGLGameEngine.Core.Windowing.Window;

namespace OpenGLGameEngine.Core;

/// <summary>
///     The class containing the game loop and creation of window.
///     <br />
///     <br />
///     * portions of the code were modified and copied from https://gist.github.com/dcronqvist/4e83dc3a4defe5780f1d4b6cac7558f6
/// </summary>
public static class GameWindow
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public static Window window { get; private set; }

    /// <summary>
    ///     Whether to use the default shader program before every render.
    ///     <br />
    ///     <br />
    ///     If you are using your own shader program, i.e you are calling Gl.UseProgram every loop before any render functions,
    ///     <br />you can set this to false to prevent unnecessarily updating opengl state.
    /// </summary>
    public static bool UseDefaultShader = true;

    static GameWindow()
    {
        CoreUtils.ConfigureNLog(logger);
        logger.Info($"GameEngine version: {CoreUtils.VERSION}");
    }

    /// <summary>
    ///     This event is invoked when the game loops exits and the window closes.
    /// </summary>
    public static event Action GameLoopExit;

    public static event Action GameLoopDraw;
    public static event Action GameLoopUpdate;


    /// <summary>
    ///     Initialises the game engine and creates the window
    /// </summary>
    /// <param name="windowTitle">The title of the window</param>
    /// <param name="fullscreenKey">The key to toggle fullscreen when pressed. Set to null to disable</param>
    /// <param name="windowMode">The display mode: windowed, maximised, fullscreen, fullscreen borderless.</param>
    /// <param name="windowSize">Size of the window in windowed mode.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Create(
        string windowTitle,
        Keys? fullscreenKey = Keys.F11,
        WindowModes windowMode = WindowModes.Windowed,
        (int width, int height) windowSize = default
    )
    {
        if (windowSize.Equals(default)) windowSize = WindowUtils.DefaultWindowSize;


        // Start initialisation and create opengl context and window.
        logger.Info("Beginning initialisation and creation of window...");

        logger.Trace("Attempting to find glfw.dll at {directory}...", Directory.GetCurrentDirectory());
        if (!Glfw.Init())
        {
            logger.Fatal("Glfw Failed to initialised!");
            return;
        }

        logger.Debug("Glfw initialised successfully!");
        Glfw.SetErrorCallback(onGlfwError);

        logger.Info("GLFW Version: {version}", Glfw.VersionString);

        logger.Trace("GLFW detected Monitors available:");
        for (var i = 0; i < Glfw.Monitors.Length; i++)
        {
            var m = Glfw.Monitors[i];
            logger.Trace("- index:{index} Resolution: {width}x{height} position {xpos},{ypos}", i, m.WorkArea.Width, m.WorkArea.Height, m.WorkArea.X, m.WorkArea.Y);
        }

        // Window and context creation
        logger.Debug("Begin window and context creation...");
        window = Window.Create(windowTitle, new WindowRect(Vector2.Zero, new Vector2(windowSize.width,windowSize.height)), windowMode, true);
        window.InitOpenGL();
        window.MakeCurrent();
        window.InitInput();
        logger.Debug("Window and context creation successful!");
        Glfw.SwapInterval(1);
        Gl.Enable(EnableCap.DepthTest);

        Texture.ConfigureOpenGl();

        logger.Info("!!!!!!!!!!!!!!!!!!!!! Initial configuration and initialisation done !!!!!!!!!!!!!!!!!!!!!");
    }



    public static void Run()
    {
        
        while (!window.shouldClose)
        {
            GameTime.UpdateDeltaTime(Glfw.Time);

            window.Poll();

            // Update game logic
            GameLoopUpdate?.Invoke();
            // Draw
            window.Clear();
            GameLoopDraw?.Invoke();
            window.SwapBuffers();
        }
        Stop();
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
        var description = Marshal.PtrToStringAnsi(description_p);
        logger.Error("Glfw has encountered an error ({errCode}) {desc}", errCode, description);
    }
}