using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Processors;

namespace OpenGLGameEngine.Components;

public abstract class EntityScript : Component
{
    public bool Enabled { get; set; }
    public override void OnAdd()
    {
        World?.GetProcessor<EntityScriptExecutor>()?.addComponent(this);
    }

    public override void OnRemove()
    {
        World?.GetProcessor<EntityScriptExecutor>()?.removeComponent(this);
    }

    /// <summary>
    /// This method is called when the engine is loading the game. This is called before the main game loop starts.
    /// Use this method to load resources or any other assets or shaders
    /// to avoid big overhead computational costs.
    /// </summary>
    public abstract void Load();
    /// <summary>
    /// Called when the entity is added to the world. (During the game loop) and after the load method is called.
    /// </summary>
    public abstract void Start();
    /// <summary>
    /// This is where you should update the entity's state / physics / etc.
    /// </summary>
    public abstract void Update();
    /// <summary>
    /// Called every frame. Put in any rendering or ui related code here.
    /// </summary>
    public abstract void Draw();
}