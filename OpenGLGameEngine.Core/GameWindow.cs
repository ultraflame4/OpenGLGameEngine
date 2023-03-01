using System.Runtime.InteropServices;
using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Graphics;
using OpenGLGameEngine.Utils;
using ErrorCode = GLFW.ErrorCode;
using Monitor = GLFW.Monitor;

namespace OpenGLGameEngine.Core;

/// <summary>
/// The class containing the game loop and creation of window.
/// <br/>
/// <br/>
/// * portions of the code were modified and copied from https://gist.github.com/dcronqvist/4e83dc3a4defe5780f1d4b6cac7558f6
/// </summary>
public static class GameWindow
{
    static Logger logger = NLog.LogManager.GetCurrentClassLogger();
    static Window window;

    /// <summary>
    /// This event is invoked when the game loops exits and the window closes.
    /// </summary>
    public static event Action GameLoopExit;

    public static event Action GameLoopDraw;
    public static event Action GameLoopUpdate;

    static GameWindow()
    {
        CoreUtils.ConfigureNLog(logger);
        logger.Info($"GameEngine version: {CoreUtils.VERSION}");
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
        logger.Info("Begin window and context creation...");
        window = WindowUtils.CreateWindow(windowTitle, windowSize, windowMode);

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
        Gl.Enable(EnableCap.DepthTest);
        
        Texture.ConfigureOpenGl();
        
        logger.Info("!!!!!!!!!!!!!!!!!!!!! Initial configuration and initialisation done !!!!!!!!!!!!!!!!!!!!!");
        logger.Info("Loading defaults...");
        LoadDefaults();
    }

    private static void LoadDefaults()
    {
        ShaderUtils.LoadDefaultShaderProgram();
    }

    public static void Run()
    {
        while (!Glfw.WindowShouldClose(window))
        {
            Glfw.SwapBuffers(window);

            GameTime.UpdateDeltaTime(Glfw.Time);

            Glfw.PollEvents();

            Update();

            int width, height;
            Glfw.GetFramebufferSize(window, out width, out height);

            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Draw();

            OpenGL.ErrorCode code;
            while ((code = Gl.GetError()) != OpenGL.ErrorCode.NoError)
            {
                logger.Error($"OpenGL Error Code: {code} !");
            }
        }

        Stop();
    }

    /// <summary>
    /// Whether to use the default shader program before every render.
    /// <br/>
    /// <br/>
    /// If you are using your own shader program, i.e you are calling Gl.UseProgram every loop before any render functions,
    /// <br/>you can set this to false to prevent unnecessarily updating opengl state.
    /// </summary>
    public static bool UseDefaultShader = true;

    private static void Draw()
    {
        // use default program
        if (UseDefaultShader) Gl.UseProgram(ShaderUtils.DefaultShaderProgram);
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
}