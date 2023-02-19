﻿namespace OpenGLGameEngine.ECS;

/// <summary>
/// A simple class to make adding / removing of components to entities easier 
/// </summary>
public class Entity
{
    private uint entity_id;
    private World worldInstance;
    /// <summary>
    /// List of components added to this entity
    /// </summary>
    private List<IComponent> components = new();
    public Entity(uint entityId, World worldInstance)
    {
        entity_id = entityId;
        this.worldInstance = worldInstance;
    }

    /// <summary>
    /// List of components added to this entity
    /// </summary>
    public List<IComponent> Components => components;

    public T AddComponent<T>(T component) where T : IComponent
    {
        component.OnAdd();
        Components.Add(component);
        return component;
    }
    
    public T RemoveComponent<T>(T component) where T : IComponent
    {
        component.OnRemove();
        Components.Remove(component);
        return component;
    }
    
    public void Destroy()
    {
        Components.ForEach(x => x.OnRemove());
    }
}