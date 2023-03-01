using System.Numerics;
using GLFW;
using NLog;
using OpenGL;
using Monitor = GLFW.Monitor;

namespace OpenGLGameEngine.Utils;

public static class WindowUtils
{
    public static (int width, int height) DefaultWindowSize = (848, 480);

    private static int last_x;
    private static int last_y;
    private static int last_w;
    private static int last_h;

    private static WindowModes currentMode = WindowModes.Windowed;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static void SetWindowHints()
    {
        // Set some common hints for the OpenGL profile creation
        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        // Set opengl version
        Glfw.WindowHint(Hint.ContextVersionMajor, 4);
        Glfw.WindowHint(Hint.ContextVersionMinor, 6);

        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.Decorated, true);
        Glfw.WindowHint(Hint.AutoIconify, false);
    }


    public static void UpdateWindowSpacialData(Window window)
    {
        Glfw.GetWindowPosition(window, out last_x, out last_y);
        Glfw.GetWindowSize(window, out last_w, out last_h);
        logger.Debug($"Saved window spacial data (position and size) position: {last_x},{last_y} size: {last_w},{last_h}");
    }

    public static (int width, int height) GetWindowSize()
    {
        return (last_w, last_h);
    }

    public static float GetAspectRatio()
    {
        var size = GetWindowSize();
        return size.width / (float)size.height;
    }

    /// <summary>
    ///     Returns the monitor closest to the window's central position.
    /// </summary>
    /// <param name="monitors">The monitors to compare</param>
    /// <param name="window">The window to use</param>
    /// <returns></returns>
    public static Monitor GetClosestMonitor(this Monitor[] monitors, Window window)
    {
        int winX, winY, winW, winH;
        Glfw.GetWindowPosition(window, out winX, out winY);
        Glfw.GetWindowSize(window, out winW, out winH);
        // Get the position in the center & reassign existing variables
        (winX, winY) = Utils.TopLeft2CenterPosition(winX, winY, winW, winH);
        // convert to vector for distance calculation
        var windowPosition = (winX, winY).xy2Vector2();

        var last_closest = Monitor.None;
        float last_dist = -1;
        for (var i = 0; i < monitors.Length; i++)
        {
            var monitor = monitors[i];
            var monitorPosition = Utils.TopLeft2CenterPosition(monitor.WorkArea.X, monitor.WorkArea.Y, monitor.WorkArea.Width, monitor.WorkArea.Height).xy2Vector2();

            var currentDist = Vector2.Distance(windowPosition, monitorPosition);

            if (currentDist < last_dist || i == 0)
            {
                last_closest = monitor;
                last_dist = currentDist;
            }
        }

        return last_closest;
    }

    public static void SetWindowDisplayMode(Window window, WindowModes mode)
    {
        if (mode == currentMode) return;

        var monitor = Glfw.Monitors.GetClosestMonitor(window);
        var videoMode = Glfw.GetVideoMode(monitor);
        switch (mode)
        {
            case WindowModes.Windowed:
                Glfw.SetWindowMonitor(window, Monitor.None, last_x, last_y, last_w, last_h, 0);

                break;
            case WindowModes.Maximised:
                UpdateWindowSpacialData(window);
                Glfw.MaximizeWindow(window);

                break;
            case WindowModes.Fullscreen:
                UpdateWindowSpacialData(window);
                Glfw.SetWindowMonitor(window, monitor, 0, 0, videoMode.Width, videoMode.Height, videoMode.RefreshRate);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }

        currentMode = mode;
    }

    public static bool IsFullscreen()
    {
        return currentMode == WindowModes.Fullscreen;
    }

    public static void ToggleFullscreen(Window window)
    {
        if (IsFullscreen())
        {
            logger.Trace("ToggleFullscreen: Going back to windowed");
            SetWindowDisplayMode(window, WindowModes.Windowed);
        }
        else
        {
            logger.Trace("ToggleFullscreen: Going to fullscreen");
            SetWindowDisplayMode(window, WindowModes.Fullscreen);
        }
    }

    public static Window CreateWindow(string windowTitle, (int width, int height) windowSize, WindowModes windowMode)
    {
        Window window;
        SetWindowHints();
        logger.Info($"- Set Window Title: {windowTitle}");
        logger.Info($"- Set Window Mode: {windowMode.ToString()}");
        logger.Info($"- Set Window Size: {windowMode.ToString()}");

        window = Glfw.CreateWindow(windowSize.width, windowSize.height, windowTitle, Monitor.None, Window.None);

        if (window == Window.None)
        {
            logger.Fatal("Window or OpenGL context failed");
            throw new InvalidOperationException("Window or OpenGL context failed");
        }

        SetWindowDisplayMode(window, windowMode);
        Gl.Initialize();
        Glfw.MakeContextCurrent(window);

        logger.Info("OpenGL Context created successfully. !");
        logger.Info("OpenGL configuration:");
        logger.Info($"- Version: {Gl.GetString(StringName.Version)}");
        logger.Info($"- ShaderLang Version: {Gl.GetString(StringName.ShadingLanguageVersion)}");
        logger.Info($"- Renderer: {Gl.GetString(StringName.Renderer)}");
        logger.Info($"- Vender: {Gl.GetString(StringName.Vendor)}");

        logger.Info("Create window success!");
        UpdateWindowSpacialData(window); // must be called so that the user's window size config is not changed


        return window;
    }
}