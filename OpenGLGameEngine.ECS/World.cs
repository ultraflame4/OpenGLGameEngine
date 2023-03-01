using NLog;

namespace OpenGLGameEngine.ECS;

/// <summary>
///     Represents a single context or world in the ECS
/// </summary>
public class World
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private List<uint> entities = new();
    private uint id_counter;
    private readonly List<IProcessor> processors = new();

    public Entity CreateEntity()
    {
        var id = id_counter;
        id_counter++;
        return new Entity(id_counter, this);
    }

    public T? GetProcessor<T>() where T : class, IProcessor, new()
    {
        foreach (var processor in processors)
            if (processor is T)
                return (T)processor;
        logger.Warn($"Processor of type {typeof(T)} not found!");

        return null;
    }

    public void AddProcessor(IProcessor processor)
    {
        processors.Add(processor);
    }

    public void RemoveProcessor(IProcessor processor)
    {
        processors.Remove(processor);
    }

    public void RunProcessors()
    {
        processors.ForEach(processor => processor.processComponents());
    }
}