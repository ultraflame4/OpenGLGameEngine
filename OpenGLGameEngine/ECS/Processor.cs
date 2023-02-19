using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.ECS;
public interface IProcessor
{
    public void processComponents();
}

public abstract class Processor<T> : IProcessor where T : IComponent
{
    public List<T> components = new List<T>();
    
    public Processor()
    {
        
    }

    public void addComponent(T component)
    {
        components.Add(component);
    }
    
    public void removeComponent(T component)
    {
        components.Remove(component);
    }
    
    public abstract void processComponents();
}

