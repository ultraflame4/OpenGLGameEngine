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
    /// <summary>
    /// Checks for any OpenGL errors and logs them.
    /// </summary>
    public static void CheckError()
    {
        OpenGL.ErrorCode code;
        while ((code = OpenGL.Gl.GetError()) != OpenGL.ErrorCode.NoError) logger.Error("OpenGL Error Code: {code} !", code);
    }

}