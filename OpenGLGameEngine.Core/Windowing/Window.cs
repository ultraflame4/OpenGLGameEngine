using System.Numerics;
using NLog;
using OpenGL;
using OpenGLGameEngine.Utils;

namespace OpenGLGameEngine.Core.Windowing;

public class Window
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    GLFW.Window glfwWindow;
    
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
        WindowUtils.SetWindowDisplayMode(glfwWindow, mode);
        currentMode = mode;
    }
    public void ToggleFullscreen(Window window)
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