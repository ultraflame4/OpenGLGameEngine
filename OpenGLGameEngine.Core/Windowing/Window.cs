using System.Numerics;
using NLog;
using OpenGL;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Core.Windowing;

public class Window
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    public readonly GLFW.Window glfwWindow;
    
    public bool IsFullscreen { get; private set; }
    WindowModes currentMode = WindowModes.Windowed;
    WindowRect current;
    WindowRect saved;


    public Window(GLFW.Window glfwWindow, WindowRect current)
    {
        this.glfwWindow=glfwWindow;
        saved = this.current = current;
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
                GLFW.Glfw.SetWindowMonitor(glfwWindow, GLFW.Monitor.None, current.X, current.Y, current.Width, current.Height, 0);
                break;
            case WindowModes.Maximised:
                saved = current;
                GLFW.Glfw.MaximizeWindow(glfwWindow);

                break;
            case WindowModes.Fullscreen:
                saved = current;
                GLFW.Glfw.SetWindowMonitor(glfwWindow, monitor, 0, 0, videoMode.Width, videoMode.Height, videoMode.RefreshRate);

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
 
    public static Window Create(string title, WindowRect rect, WindowModes windowMode = WindowModes.Windowed)
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
        
        Gl.Initialize();
        GLFW.Glfw.MakeContextCurrent(glfwWindow);
        logger.Debug("OpenGL Context created successfully. !");
        logger.Trace("OpenGL configuration:");
        logger.Trace($"- Version: {Gl.GetString(StringName.Version)}");
        logger.Trace($"- ShaderLang Version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
        logger.Trace($"- Renderer: {Gl.GetString(StringName.Renderer)}");
        logger.Trace($"- Vender: {Gl.GetString(StringName.Vendor)}");
        Window window = new(glfwWindow, rect);
        logger.Info("Create window success!");
        
        return window;
    }
}