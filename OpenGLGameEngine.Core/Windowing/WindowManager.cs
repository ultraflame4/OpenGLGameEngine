using System.Runtime.InteropServices;
using GLFW;
using NLog;

namespace OpenGLGameEngine.Core.Windowing;

public static class WindowManager
{
    private static Logger logger = LogManager.GetCurrentClassLogger();

    private static void InitGlfw()
    {
        logger.Trace("Attempting to initialise glfw...");
        logger.Trace("- Expecting glfw.dll at {directory}", Directory.GetCurrentDirectory());
        if (!Glfw.Init())
        {
            logger.Fatal("Glfw Failed to initialised!");
            throw new InvalidOperationException("Failed to initialise glfw!");
        }
        Glfw.SetErrorCallback(onGlfwError);
        logger.Info("GLFW Version: {version}", Glfw.VersionString);
        logger.Trace("GLFW detected Monitors available:");
        for (var i = 0; i < Glfw.Monitors.Length; i++)
        {
            var m = Glfw.Monitors[i];
            logger.Trace("- index:{index} Resolution: {width}x{height} position {xpos},{ypos}", i, m.WorkArea.Width, m.WorkArea.Height, m.WorkArea.X, m.WorkArea.Y);
        }
        logger.Debug("Glfw initialised successfully!");
    }
    private static void onGlfwError(ErrorCode errCode, IntPtr description_p)
    {
        var description = Marshal.PtrToStringAnsi(description_p);
        logger.Error("Glfw has encountered an error ({errCode}) {desc}", errCode, description);
    }
    
    public static void Init()
    {
        logger.Info("Begin initialisation of windowing system...");
        InitGlfw();
        logger.Info("Finished initialisation of windowing system!");
    }

    public static void Terminate()
    {
        logger.Debug("Terminating windowing system...");
        Glfw.Terminate();
        logger.Info("Terminated windowing system!");
    }
}