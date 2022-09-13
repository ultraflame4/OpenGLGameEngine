using NLog;

namespace OpenGLGameEngine.Utils;

public static class GameTime
{
    private static float _deltatime = 0;
    private static double _lastTotalElapsed = 0;
    static Logger logger = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// Returns the current delta time, the time between each frame
    /// </summary>
    public static float DeltaTime => _deltatime;
    /// <summary>
    /// Returns the total time elapsed from beginning of the current frame.
    /// </summary>
    public static double LastFrameTime => _lastTotalElapsed;

    public static float CurrentFPS => 1 / _deltatime;
    public static void UpdateDeltaTime(double currentTime)
    {
        _deltatime = (float)(currentTime - _lastTotalElapsed);
        _lastTotalElapsed = currentTime;

        if (_deltatime > 1)
        {
            logger.Warn($"Lag Alert! Current FPS is less than 1! Current deltatime: {DeltaTime}; fps: {CurrentFPS}");
        }
    }
}