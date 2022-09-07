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


    public static void SetWindowFullscreen(bool fullscreen, Window window)
    {
        if (IsWindowFullscreen(window) == fullscreen)
            return;

        var monitor = Glfw.PrimaryMonitor;
        if (fullscreen)
        {
            UpdateWindowSpacialData(window); // back up current window size and position
            var videoMode = Glfw.GetVideoMode(monitor);
            Glfw.SetWindowMonitor(window,monitor,0,0,videoMode.Width,videoMode.Height,0);
        }
        else
        {
            //restore
            Glfw.SetWindowMonitor(window,Monitor.None, last_x,last_y,last_w,last_h,0);
        }
    }

    public static bool IsWindowFullscreen(Window window)
    {
        return Glfw.GetWindowMonitor(window) != Monitor.None;
    }

    public static void UpdateWindowSpacialData(Window window)
    {
         Glfw.GetWindowPosition(window,out last_x,out last_y);
        Glfw.GetWindowSize(window,out last_w,out last_h);
        
    }
}