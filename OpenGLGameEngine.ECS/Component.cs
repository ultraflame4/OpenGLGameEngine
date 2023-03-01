namespace OpenGLGameEngine.ECS;

public interface IComponent
{
    bool Enabled { get; set; }

    public void AddToEntity(Entity entity);
    public void RemoveFromEntity();
}

public abstract class Component : IComponent
{
    public Entity? AttachedEntity { get; private set; }
    public bool Enabled { get; set; }

    public void AddToEntity(Entity entity)
    {
        AttachedEntity = entity;
        Enabled = true;
        OnAdd();
    }

    public void RemoveFromEntity()
    {
        AttachedEntity = null;
        OnRemove();
    }

    /// <summary>
    /// Called when the component is added to an entity. If needed, this should add itself to the processors.
    /// </summary>
    public abstract void OnAdd();

    /// <summary>
    /// Called when the component is removed from the entity or when entity is destroyed. If needed, this should remove itself from the processors.
    /// </summary>
    public abstract void OnRemove();

}