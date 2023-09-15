using System.Numerics;
using GLFW;
using NLog;
using OpenGLGameEngine.Core.Windowing;
using Monitor = GLFW.Monitor;

namespace OpenGLGameEngine.Core.Utils;

public static class WindowUtils
{
    public static (int width, int height) DefaultWindowSize = (848, 480);

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


    /// <summary>
    ///     Returns the monitor closest to the window's central position.
    /// </summary>
    /// <param name="windowRect">Rect of the window</param>
    /// <returns></returns>
    public static Monitor GetClosestMonitor( WindowRect windowRect)
    {
        var monitors = Glfw.Monitors;
        var last_closest = Monitor.None;
        float last_dist = -1;
        for (var i = 0; i < monitors.Length; i++)
        {
            var monitor = monitors[i];
            var monitorRect = new WindowRect(monitor.WorkArea.X, monitor.WorkArea.Y, monitor.WorkArea.Width,monitor.WorkArea.Height);
            var currentDist = Vector2.Distance(windowRect.center, monitorRect.center);

            if (currentDist < last_dist || i == 0)
            {
                last_closest = monitor;
                last_dist = currentDist;
            }
        }

        return last_closest;
    }
           
}