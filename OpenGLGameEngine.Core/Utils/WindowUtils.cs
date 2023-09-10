using System.Numerics;
using GLFW;
using NLog;
using OpenGLGameEngine.Core.Windowing;
using Monitor = GLFW.Monitor;
using Window = GLFW.Window;

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
           
}