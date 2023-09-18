using NLog;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Core.Windowing;

namespace OpenGLGameEngine.Core;

/// <summary>
/// Represents a game window that is accessible by the user / Universe System.
/// Should be created using <see cref="Create"/> method.
/// Todo - further improve this class to eventually replace MainWindow.
/// todo - Maybe put universe system in here? and have it auto draw and update? would cut down on files & function / event calls inbetween.
/// </summary>
public sealed class GameWindow
{
    public static GLFW.Keys? fullscreenKey = GLFW.Keys.F11;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private Window window;
    private double lastFrameTime;
    private TickTimer fpsTimer = new TickTimer();

    private GameWindow(Window window) { this.window = window; }

    public static GameWindow Create(
        string title,
        (int width, int height)? size = null,
        bool fullscreen = false
    )
    {
        var windowSize = size ?? WindowUtils.DefaultWindowSize;
        var window = Window.Create(
            title,
            new WindowRect(0, 0, windowSize.width, windowSize.height),
            fullscreen ? WindowModes.Fullscreen : WindowModes.Windowed,
            true
        );
        window.Init();
        window.InitInput();
        logger.Info($"Created window with title: {title}, size: {windowSize.width}x{windowSize.height}, fullscreen: {fullscreen}");
        return new GameWindow(window);
    }
    
    public void Run(Action update, Action draw)
    {
        fpsTimer.Start();
        while (!window.shouldClose)
        {
            window.Poll();
            fpsTimer.Tick();
            update();
            window.Clear();
            draw();
            window.SwapBuffers();
        }
    }
}