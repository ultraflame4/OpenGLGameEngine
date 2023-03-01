namespace OpenGLGameEngine.ECS;

/// <summary>
///     A simple class to make adding / removing of components to entities easier
/// </summary>
public class Entity
{
    /// <summary>
    ///     List of components added to this entity
    /// </summary>
    private readonly List<IComponent> components = new();

    private uint entity_id;

    public Entity(uint entityId, World worldInstance)
    {
        entity_id = entityId;
        this.worldInstance = worldInstance;
    }

    public World worldInstance { get; }

    /// <summary>
    ///     List of components added to this entity
    /// </summary>
    public List<IComponent> Components => components;

    public T AddComponent<T>(T component) where T : IComponent
    {
        component.AddToEntity(this);
        Components.Add(component);
        return component;
    }

    public T RemoveComponent<T>(T component) where T : IComponent
    {
        component.RemoveFromEntity();
        Components.Remove(component);
        return component;
    }

    public T? GetComponent<T>() where T : class, IComponent
    {
        foreach (var component in Components)
            if (component.GetType() == typeof(T))
                return (T)component;
        return null;
    }

    public void Destroy()
    {
        Components.ForEach(x => x.RemoveFromEntity());
    }
}