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
        Remove();
    }

    /// <summary>
    /// This method is called when the engine is loading the game. This is called before the main game loop starts.
    /// Use this method to load resources or any other assets
    /// that have big overhead computational costs <br/>
    /// Also use this method when you need to add components (initially) <br/>
    /// Not all components have been added to the entity yet. <br/>
    /// </summary>
    public virtual void Load() { }

    /// <summary>
    /// Called when the entity is added to the world. (During the game loop) and after the load method is called.<br/>
    /// Use this method when you need to use components that are added to the entity.
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

    /// <summary>
    /// Called after this component is removed from the processors due to various reasons (deletion of entity, removal of component etc.)
    /// Put in any cleanup code here to prevent memory leaks.
    /// </summary>
    public virtual void Remove() { }

}