namespace OpenGLGameEngine.EntityComponentSys;

/// <summary>
/// A simple class to make adding / removing of components to entities easier 
/// </summary>
public class EntityHandler
{
    private uint entity_id;
    public EntityHandler(uint entityId)
    {
        entity_id = entityId;
    }
}