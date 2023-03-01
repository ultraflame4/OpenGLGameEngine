using NLog;

namespace OpenGLGameEngine.Utils;

public static class GameTime
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    ///     Returns the current delta time, the time between each frame
    /// </summary>
    public static float DeltaTime { get; private set; }

    /// <summary>
    ///     Returns the total time elapsed from beginning of the current frame.
    /// </summary>
    public static double LastFrameTime { get; private set; }

    public static float CurrentFPS => 1 / DeltaTime;

    public static void UpdateDeltaTime(double currentTime)
    {
        DeltaTime = (float)(currentTime - LastFrameTime);
        LastFrameTime = currentTime;

        if (DeltaTime > 1) logger.Warn($"Lag Alert! Current FPS is less than 1! Current deltatime: {DeltaTime}; fps: {CurrentFPS}");
    }
}