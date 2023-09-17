using System.Numerics;
using NLog;
using OpenGL;
using OpenGLGameEngine.Core.Drawing;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Inputs;

namespace OpenGLGameEngine.Core.Windowing;
/// <summary>
/// This class is used to create a GLFW window and OpenGL context.
/// It also handles window resizing and fullscreen.
/// </summary>
public class Window : GraphicalContext
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly Logger logger;
    public static GLFW.Keys fullscreenKey = GLFW.Keys.F11;

    public readonly GLFW.Window glfwWindow;
    public bool enableFullscreen = false;

    /// <summary>
    /// Whether the window is fullscreen or not.
    /// </summary>
    public bool IsFullscreen => currentMode == WindowModes.Fullscreen;

    /// <summary>
    /// Shorthand for <see cref="GLFW.Glfw.WindowShouldClose"/>
    /// </summary>
    public bool shouldClose => GLFW.Glfw.WindowShouldClose(glfwWindow);

    WindowModes currentMode = WindowModes.Windowed;
    public WindowRect CurrentRect { get; private set; }
    public WindowRect SavedRect { get; private set; }


    public Window(GLFW.Window glfwWindow, string title, WindowRect currentRect)
    {

        logger = LogManager.GetLogger($"{GetType().FullName} [{title}]");
        this.glfwWindow = glfwWindow;
        SavedRect = CurrentRect = currentRect;
    }

    public void UpdateRectData()
    {
        int x, y, w, h;
        GLFW.Glfw.GetWindowPosition(glfwWindow, out x, out y);
        GLFW.Glfw.GetWindowSize(glfwWindow, out w, out h);
        CurrentRect = new WindowRect(x, y, w, h);
    }

    public void UpdateDisplayMode(WindowModes mode)
    {
        if (mode == currentMode) return;
        var monitor = WindowUtils.GetClosestMonitor(CurrentRect);
        var videoMode = GLFW.Glfw.GetVideoMode(monitor);
        switch (mode)
        {
            case WindowModes.Windowed:
                CurrentRect = SavedRect;
                GLFW.Glfw.SetWindowMonitor(glfwWindow, GLFW.Monitor.None, CurrentRect.X, CurrentRect.Y,
                    CurrentRect.Width,
                    CurrentRect.Height, 0);
                break;
            case WindowModes.Maximised:
                SavedRect = CurrentRect;
                GLFW.Glfw.MaximizeWindow(glfwWindow);

                break;
            case WindowModes.Fullscreen:
                SavedRect = CurrentRect;
                GLFW.Glfw.SetWindowMonitor(glfwWindow, monitor, 0, 0, videoMode.Width, videoMode.Height,
                    videoMode.RefreshRate);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }

        currentMode = mode;
    }

    public void ToggleFullscreen()
    {
        if (IsFullscreen)
        {
            logger.Trace("ToggleFullscreen - Going back to windowed");
            UpdateDisplayMode(WindowModes.Windowed);
        }
        else
        {
            logger.Trace("ToggleFullscreen - Going to fullscreen");
            UpdateDisplayMode(WindowModes.Fullscreen);
        }
    }

    /// <summary>
    /// Creates a new window with the specified title, rect and window mode.
    /// </summary>
    /// <param name="title">Title of the window</param>
    /// <param name="rect">Position and size of window</param>
    /// <param name="windowMode">Mode to start the window in</param>
    /// <param name="enableFullscreen">Whether to allow fullscreen with f11.
    /// Fullscreen can still be set using <see cref="ToggleFullscreen"/> or
    /// <see cref="UpdateDisplayMode"/> regardless if true or false</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Window Create(string title, WindowRect rect, WindowModes windowMode = WindowModes.Windowed,
        bool enableFullscreen = false)
    {
        GLFW.Window glfwWindow;
        _logger.Trace($"- Set Window Title: {title}");
        _logger.Trace($"- Set Window Mode: {windowMode.ToString()}");
        _logger.Trace($"- Set Window rect: {rect.ToString()}");
        SetWindowHints();
        glfwWindow = GLFW.Glfw.CreateWindow(rect.Width, rect.Height, title, GLFW.Monitor.None, GLFW.Window.None);

        if (glfwWindow == GLFW.Window.None)
        {
            _logger.Fatal("Failed to create window!");
            throw new InvalidOperationException("Window or OpenGL context failed");
        }

        Window window = new(glfwWindow, title, rect);
        window.enableFullscreen = enableFullscreen;
        window.RegisterCallbacks();

        _logger.Info("Create window success!");

        return window;
    }

    void RegisterCallbacks()
    {
        GLFW.Glfw.SetFramebufferSizeCallback(glfwWindow, (window, width, height) => UpdateRectData());
    }

    /// <summary>
    /// Initialise OpenGL context and set current context to this window for the current thread.
    /// </summary>
    public void Init()
    {
        Gl.Initialize();
        MakeCurrent();
        logger.Debug("OpenGL Context initialised successfully. !");
        logger.Trace("OpenGL configuration:");
        logger.Trace($"- Version: {Gl.GetString(StringName.Version)}");
        logger.Trace($"- ShaderLang Version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
        logger.Trace($"- Renderer: {Gl.GetString(StringName.Renderer)}");
        logger.Trace($"- Vender: {Gl.GetString(StringName.Vendor)}");
        Configure();
    }

    /// <summary>
    /// Initialise keyboard input and configure defaults. (NOT THREAD SAFE)
    /// </summary>
    public void InitInput()
    {
        logger.Debug("Initialisng keyboard input and configuring defaults");
        KeyboardMouseInput.Init(glfwWindow);
        logger.Trace("Set toggle fullscreen key: {fullscreenKey}", fullscreenKey);
        KeyboardMouseInput.OnKeyDown += (key, code, state, mods) =>
        {
            if (key == fullscreenKey && enableFullscreen) ToggleFullscreen();
        };
    }

    /// <summary>
    /// Makes this window the current opengl context for the current thread.
    /// </summary>
    public void MakeCurrent()
    {
        if (GLFW.Glfw.CurrentContext == glfwWindow) return;
        GLFW.Glfw.MakeContextCurrent(glfwWindow);
        GLFW.Glfw.SwapInterval(1);
        logger.Trace("glfw set current window context");
    }

    /// <summary>
    /// Poll and Updates this window, check for input, poll events, update viewport, etc.
    /// </summary>
    public void Poll()
    {
        GLFW.Glfw.PollEvents();
        KeyboardMouseInput.Update();
        GLFW.Glfw.GetFramebufferSize(glfwWindow, out int width, out int height);
        Gl.Viewport(0, 0, width, height);
        UpdateRectData();
        CheckError();
    }

    /// <summary>
    /// Swap the front and back buffers of this window. Shorthand for <see cref="GLFW.Glfw.SwapBuffers"/>
    /// </summary>
    public void SwapBuffers()
    {
        GLFW.Glfw.SwapBuffers(glfwWindow);
    }
}