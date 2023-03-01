using OpenGLGameEngine.Components;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Processors;

public class EntityScriptExecutor : Processor<EntityScript>
{
    public override void processComponents() { }

    public void ProcessUpdates()
    {
        components.ForEach(script => script.Update());
    }

    public void ProcessDraws()
    {
        components.ForEach(script => script.Draw());
    }
}