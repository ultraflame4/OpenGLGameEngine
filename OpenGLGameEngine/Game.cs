using GLFW;
using OpenGLGameEngine.Core;

namespace OpenGLGameEngine;

public static class Game
{
    public static GameWorld? CurrentWorld { get; private set; }

    public static void CreateMainWindow(string windowTitle, Keys? fullscreenKey = Keys.F11, WindowModes windowMode = WindowModes.Windowed, (int width, int height) windowSize = default)
    {
        GameWindow.Create(windowTitle, fullscreenKey, windowMode, windowSize);
    }

    public static GameWorld GetCurrentWorld()
    {
        if (CurrentWorld == null) CurrentWorld = new GameWorld();
        return CurrentWorld;
    }


    public static void Start()
    {
        GameWindow.Run();
    }
}