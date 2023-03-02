using NLog;

namespace OpenGLGameEngine.ECS;

public interface IComponent
{
    bool Enabled { get; set; }

    public void AddToEntity(Entity entity);
    public void RemoveFromEntity();
}

public abstract class Component : IComponent
{
    public Entity? Entity { get; private set; }

    public World? World => Entity?.worldInstance;

    private Logger? _logger;

    protected Logger logger => _logger ??= LogManager.GetLogger(GetType().FullName);
    

    public bool Enabled { get; set; }

    public void AddToEntity(Entity entity)
    {
        Enabled = true;
        Entity = entity;
        OnAdd();
    }

    public void RemoveFromEntity()
    {
        Entity = null;
        OnRemove();
    }

    /// <summary>
    ///     Called when the component is added to an entity. If needed, this should add itself to the processors.
    /// </summary>
    public abstract void OnAdd();

    /// <summary>
    ///     Called when the component is removed from the entity or when entity is destroyed. If needed, this should remove itself from the processors.
    /// </summary>
    public abstract void OnRemove();
}