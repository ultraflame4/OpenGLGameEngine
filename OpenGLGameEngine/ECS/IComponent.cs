namespace OpenGLGameEngine.ECS;

public interface IComponent
{
    
    bool Enabled { get; set; }
    
    /// <summary>
    /// Called when the component is added to an entity. If needed, this should add itself to the processors.
    ///
    /// It is called before the component is added to the entity.
    /// </summary>
    public void OnAdd();
    
    /// <summary>
    /// Called when the component is removed from the entity or when entity is destroyed. If needed, this should remove itself from the processors.
    ///
    /// It is called before component is removed from the entity.
    /// </summary>
    public void OnRemove();
}