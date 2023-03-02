using System.Runtime.InteropServices;
using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine.Utils;
using Monitor = GLFW.Monitor;

namespace OpenGLGameEngine.Core.Windowing;

public class Window
{
    private int width;
    private int height;
    
    private static int last_x;
    private static int last_y;
    private static int last_w;
    private static int last_h;
    
    private string title;
    private Keys? fullscreenKey;
    
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    
    private GLFW.Window glfwWindow;
    private WindowModes windowMode;

    public Window(string title, Keys? fullscreenKey = Keys.F11, WindowModes windowMode = WindowModes.Windowed, (int width, int height)? windowSize = null)
    {
        (width, height) = windowSize ?? WindowUtils.DefaultWindowSize;
        this.fullscreenKey = fullscreenKey;
        this.title = title;
        this.windowMode = windowMode;
    }

    public static void Init()
    {
        logger.Trace("Attempting to find glfw.dll at {directory}...", Directory.GetCurrentDirectory());
        if (!Glfw.Init())
        {
            logger.Fatal("Glfw Failed to initialised!");
            return;
        }

        Glfw.SetErrorCallback((code, description_p) =>
        {
            var description = Marshal.PtrToStringAnsi(description_p);
            logger.Error("Glfw has encountered an error ({errCode}) {desc}", code, description);
        });
        
        logger.Info("GLFW Version: {version}", Glfw.VersionString);

        logger.Trace("GLFW detected Monitors available:");
        for (var i = 0; i < Glfw.Monitors.Length; i++)
        {
            var m = Glfw.Monitors[i];
            logger.Trace("- index:{index} Resolution: {width}x{height} position {xpos},{ypos}", i, m.WorkArea.Width, m.WorkArea.Height, m.WorkArea.X, m.WorkArea.Y);
        }
        
    }
    

    public void Start()
    {
        
        // Start initialisation and create opengl context and window.
        logger.Info("Begin creation of window {title}",title);
        logger.Trace($"- Set Window Title: {title}");
        logger.Trace($"- Set Window Mode: {windowMode.ToString()}");
        logger.Trace($"- Set Window Size: {windowMode.ToString()}");
        WindowUtils.SetWindowHints();
        
        // modified from WindowUtils.CreateWindow
        glfwWindow = Glfw.CreateWindow(width, height, title, Monitor.None, GLFW.Window.None);
        if (glfwWindow == GLFW.Window.None)
        {
            logger.Fatal("Window creation failed");
            throw new InvalidOperationException("Window or OpenGL context failed");
        }
        
        WindowUtils.SetWindowDisplayMode(glfwWindow, windowMode);
        SaveWindowSpacialData();
        //END modified from WindowUtils.CreateWindow
        

        logger.Debug("Configuring and initiating keyboard input");
        KeyboardMouseInput.Init(glfwWindow);

        logger.Trace("Set toggle fullscreen key: {fullscreenKey}", fullscreenKey);
    }

    private void SaveWindowSpacialData()
    {
        Glfw.GetWindowPosition(glfwWindow, out last_x, out last_y);
        Glfw.GetWindowSize(glfwWindow, out last_w, out last_h);
        logger.Trace($"Saved window spacial data (position and size) position: {last_x},{last_y} size: {last_w},{last_h}");
    }

    public event Action DrawThreadStart;
    
    private void DrawThread()
    {
        
        Glfw.MakeContextCurrent(glfwWindow);
        Gl.BindAPI(); // REQUIRED SO THAT GL. IS NOT NULL AND IS BOUND TO THE CONTEXT
        
        logger.Debug("OpenGL Context created successfully. !");
        logger.Trace("OpenGL configuration:");
        logger.Trace($"- Version: {Gl.GetString(StringName.Version)}");
        logger.Trace($"- ShaderLang Version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
        logger.Trace($"- Renderer: {Gl.GetString(StringName.Renderer)}");
        logger.Trace($"- Vender: {Gl.GetString(StringName.Vendor)}");
        DrawThreadStart?.Invoke();
        
        
        
    }

    public void Run()
    {

        var drawThread = new Thread(DrawThread);
        drawThread.Start();
        while (!Glfw.WindowShouldClose(glfwWindow))
        {
            Glfw.PollEvents();
            
            Glfw.GetFramebufferSize(glfwWindow, out width, out height);

            Gl.Viewport(0, 0, width, height);
            
        }
        drawThread.Join();
    }

    private void UpdateThread() { }

    private void WindowThread() { }
}