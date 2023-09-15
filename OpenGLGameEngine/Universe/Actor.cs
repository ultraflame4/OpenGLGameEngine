
namespace OpenGLGameEngine.Universe;

public class Actor
{
    public World World { get; private set; }
    public TransformNode transform { get; private set; } = new();
    public bool Enabled;

    public void Init(World world)
    {
        World = world;
        Enabled = true;
    }
    /// <summary>
    /// Called once before the first frame
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// Called whenever the physics engine ticks. (Not every frame)
    /// </summary>
    public virtual void UpdateTick() { }

    /// <summary>
    /// Called every frame in the render thread.
    /// </summary>
    public virtual void DrawTick() { }
    
    /// <summary>
    /// Called once when the actor is removed or the world is unloaded.
    /// </summary>
    public virtual void OnRemove() { }
}