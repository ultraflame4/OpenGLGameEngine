using System.Numerics;
using NLog;
using OpenGL;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Core.Windowing;

public class Window
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
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
    WindowRect current;
    WindowRect saved;
    

    public Window(GLFW.Window glfwWindow, WindowRect current)
    {
        this.glfwWindow = glfwWindow;
        saved = this.current = current;
    }

    public void UpdateRectData()
    {
        int x, y, w, h;
        GLFW.Glfw.GetWindowPosition(glfwWindow, out x, out y);
        GLFW.Glfw.GetWindowSize(glfwWindow, out w, out h);
        current = new WindowRect(new Vector2(x, y), new Vector2(w, h));
    }

    public void UpdateDisplayMode(WindowModes mode)
    {
        if (mode == currentMode) return;
        var monitor = GLFW.Glfw.Monitors.GetClosestMonitor(glfwWindow);
        var videoMode = GLFW.Glfw.GetVideoMode(monitor);
        switch (mode)
        {
            case WindowModes.Windowed:
                current = saved;
                GLFW.Glfw.SetWindowMonitor(glfwWindow, GLFW.Monitor.None, current.X, current.Y, current.Width,
                    current.Height, 0);
                break;
            case WindowModes.Maximised:
                saved = current;
                GLFW.Glfw.MaximizeWindow(glfwWindow);

                break;
            case WindowModes.Fullscreen:
                saved = current;
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


    public static Window Create(string title, WindowRect rect, WindowModes windowMode = WindowModes.Windowed, bool enableFullscreen = false)
    {
        GLFW.Window glfwWindow;
        logger.Trace($"- Set Window Title: {title}");
        logger.Trace($"- Set Window Mode: {windowMode.ToString()}");
        logger.Trace($"- Set Window rect: {rect.ToString()}");
        WindowUtils.SetWindowHints();
        glfwWindow = GLFW.Glfw.CreateWindow(rect.Width, rect.Height, title, GLFW.Monitor.None, GLFW.Window.None);

        if (glfwWindow == GLFW.Window.None)
        {
            logger.Fatal("Failed to create window!");
            throw new InvalidOperationException("Window or OpenGL context failed");
        }
        Window window = new(glfwWindow, rect);
        window.enableFullscreen = enableFullscreen;
        window.RegisterCallbacks();
        
        logger.Info("Create window success!");

        return window;
    }

    void RegisterCallbacks()
    {
        GLFW.Glfw.SetFramebufferSizeCallback(glfwWindow, (window, width, height) => UpdateRectData());
    }

    public void InitOpenGL()
    {
        Gl.Initialize();
        MakeCurrent();
        logger.Debug("OpenGL Context initialised successfully. !");
        logger.Trace("OpenGL configuration:");
        logger.Trace($"- Version: {Gl.GetString(StringName.Version)}");
        logger.Trace($"- ShaderLang Version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
        logger.Trace($"- Renderer: {Gl.GetString(StringName.Renderer)}");
        logger.Trace($"- Vender: {Gl.GetString(StringName.Vendor)}");
    }
    
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
    public void MakeCurrent()
    {
        if (GLFW.Glfw.CurrentContext == glfwWindow) return;
        GLFW.Glfw.MakeContextCurrent(glfwWindow);
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
        ErrorCode code;
        while ((code = Gl.GetError()) != ErrorCode.NoError) logger.Error("OpenGL Error Code: {code} !", code);
    }
    /// <summary>
    /// Clears the buffers for drawing. Shorthand for <see cref="Gl.Clear"/>
    /// </summary>
    public void Clear()
    {
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
    /// <summary>
    /// Swap the front and back buffers of this window. Shorthand for <see cref="GLFW.Glfw.SwapBuffers"/>
    /// </summary>
    public void SwapBuffers()
    {
        GLFW.Glfw.SwapBuffers(glfwWindow);
    }
}