using GLFW;
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

    public static void SetWindowHints()
    {
        // Set some common hints for the OpenGL profile creation
        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.Decorated, true);
        Glfw.WindowHint(Hint.AutoIconify,false);
    }
    

    public static void UpdateWindowSpacialData(Window window)
    {
        Glfw.GetWindowPosition(window, out last_x, out last_y);
        Glfw.GetWindowSize(window, out last_w, out last_h);
    }

    public static void SetWindowDisplayMode(Window window, WindowModes mode)
    {
        if (mode == currentMode) return;

        var monitor = Glfw.PrimaryMonitor;
        VideoMode videoMode = Glfw.GetVideoMode(monitor);
        switch (mode)
        {
            case WindowModes.Windowed:
                Glfw.SetWindowMonitor(window, Monitor.None, last_x, last_y, last_w, last_h, 0);
                UpdateWindowSpacialData(window);
                break;
            case WindowModes.Maximised:
                Glfw.MaximizeWindow(window);
                UpdateWindowSpacialData(window);
                break;
            case WindowModes.Fullscreen:
                Glfw.SetWindowMonitor(window, monitor, 0, 0, videoMode.Width, videoMode.Height, videoMode.RefreshRate);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }

        currentMode = mode;
    }
}