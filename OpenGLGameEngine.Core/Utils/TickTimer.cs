

using GLFW;

namespace OpenGLGameEngine.Core.Utils;

public sealed class TickTimer
{
    public double startTime { get; private set; }
    public double lastTickTime { get; private set; }
    public float deltaTime { get; private set; }
    /// <summary>
    /// Current ticks per second
    /// </summary>
    public float TPS => 1 / deltaTime;
    
    public TickTimer() { }

    public void Start() { lastTickTime = startTime = Glfw.Time; }

    public void Tick()
    {
        double now = Glfw.Time;
        deltaTime = (float)(now-lastTickTime);
        lastTickTime = now;
    }
}