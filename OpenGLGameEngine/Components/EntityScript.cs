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
    /// This method is called when the script 
    /// </summary>
    public abstract void Load();
    public abstract void Start();
    public abstract void Update();
    public abstract void Draw();
}