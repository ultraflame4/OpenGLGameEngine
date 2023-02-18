namespace OpenGLGameEngine.ECS;

/// <summary>
/// Represents a single context or world in the ECS
/// </summary>
public class World
{
    uint id_counter = 0;

    private List<uint> entities = new();

    public Entity createEntity()
    {
        uint id = id_counter;
        id_counter++;
        return new Entity(id_counter, this);
    }

    /// <summary>
    /// Adds a component to the entity.
    /// 
    /// Note that it is completely up to the user to keep track of what components are on what entities. And what components each entities have.
    /// </summary>
    /// <param name="componentInstance">The component to add to the entity</param>
    /// <returns>The component that was added</returns>
    public T EntityAddComponent<T>(T componentInstance) where T : IComponent
    {
        return componentInstance;
    }
    
    
}