using System.Runtime.InteropServices;

namespace OpenGLGameEngine;
using GLFW;
        
public static class GameEngine
{

    private static void glfwErrorCallback(ErrorCode error, IntPtr description)
    {
        string? d = Marshal.PtrToStringAuto(description);
        Console.Error.WriteLine($"GLFW has encountered an error:{error} {d}");
    }

    public static void Init()
    {
        Console.WriteLine($"Finding GLFW at {Glfw.LIBRARY}");
        if (!Glfw.Init())
        {
            Console.Error.WriteLine("Fatal Error: GLFW failed to initialised.");
            return;
        }

        Glfw.SetErrorCallback(glfwErrorCallback);
    }
    
    /**
     * Starts the main game loop.
     * 
     * Should be the last method called.
     *
     * This method is not thread safe
     */
    public static void Run()
    {

        
        Glfw.Terminate();
    }
    
}