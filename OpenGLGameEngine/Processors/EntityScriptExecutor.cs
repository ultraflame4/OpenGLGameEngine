using OpenGLGameEngine.Components;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Processors;

public class EntityScriptExecutor : Processor<EntityScript>
{
    public override void processComponents() { }
    private bool started = false;

    public override void addComponent(EntityScript component)
    {
        // Load whatever resources the script needs.
        component.Load();
        base.addComponent(component);
        // If started, immediately start the script.
        if (started) { component.Start(); }
    }
    
    public void ProcessStarts()
    {
        // If already started, return. preventing double starting
        if (started) { return; }
        // Set started to true so that any scripts added after this will be started immediately.
        started = true;
        // Start all scripts.
        components.ForEach(script => script.Start());
    }

    public void ProcessUpdates()
    {
        components.ForEach(script => script.Update());
    }

    public void ProcessDraws()
    {
        components.ForEach(script => script.Draw());
    }
}