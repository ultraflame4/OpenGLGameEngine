using System.Collections;

namespace OpenGLGameEngine.EntityComponentSys;

/// <summary>
/// Represents a single context or world in the ECS
/// </summary>
public class World
{
    uint id_counter = 0;

    private List<uint> entities = new();

    public EntityHandler createEntity()
    {
        uint id = id_counter;
        id_counter++;
        return new EntityHandler(id_counter);
    }
    
    
}