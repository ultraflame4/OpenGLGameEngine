namespace OpenGLGameEngine.Universe;

public class World
{
    private List<Actor> actors = new();
    private bool started = false;
    
    public void AddActor(Actor actor)
    {
        actors.Add(actor);
        actor.Init(this);
        if (started) actor.Start();
    }
    public void RemoveActor(Actor actor)
    {
        actors.Remove(actor);
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