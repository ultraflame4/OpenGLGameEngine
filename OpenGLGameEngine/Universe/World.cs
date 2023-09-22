namespace OpenGLGameEngine.Universe;

public class World
{
    private List<Actor> actors = new();
    private bool started = false;

    /// <summary>
    /// Adds an actor to the world.
    /// </summary>
    /// <param name="actor">Actor to add to the world</param>
    /// <param name="parent">Parent of actor</param>
    /// <returns>returns the actor that was added</returns>
    public Actor AddActor(Actor actor, TransformNode? parent = null)
    {
        if (actors.Contains(actor)) return actor;
        actors.Add(actor);
        actor.Init(this, parent);
        if (started) actor.Start();
        return actor;
    }
    public void RemoveActor(Actor actor)
    {
        actors.Remove(actor);
        actor.transform.SetParent(null);
        actor.OnRemove();
    }
    
    public void Start()
    {
        started = true;
        foreach (var actor in actors)
        {
            actor.Start();
        }
    }
    public void TickUpdate()
    {
        foreach (var actor in actors)
        {
            actor.UpdateTick();
        }
    }
    public void TickDraw()
    {
        foreach (var actor in actors)
        {
            actor.DrawTick();
        }
    }
    
    public void Remove()
    {
        foreach (var actor in actors)
        {
            actor.OnRemove();
        }
    }
}