namespace OpenGLGameEngine.Utils;

public static class GameTimer
{
    public static float _deltatime = 0;
    public static double _lastTotalElapsed = 0;

    /// <summary>
    /// Returns the current delta time, the time between each frame
    /// </summary>
    public static float DeltaTime => _deltatime;
    /// <summary>
    /// Returns the total time elapsed from beginning of the current frame.
    /// </summary>
    public static double LastFrameTime => _lastTotalElapsed;
    
}