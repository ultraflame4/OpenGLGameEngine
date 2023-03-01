namespace OpenGLGameEngine.ECS;

public interface IProcessor
{
    public void processComponents();
}

public abstract class Processor<T> : IProcessor where T : IComponent
{
    public List<T> components = new();

    public abstract void processComponents();

    public virtual void addComponent(T component)
    {
        components.Add(component);
    }

    public virtual void removeComponent(T component)
    {
        components.Remove(component);
    }
}