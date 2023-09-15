namespace OpenGLGameEngine.Universe;

public class World
{
    private List<Actor> actors = new();
    
    public void AddActor(Actor actor)
    {
        actors.Add(actor);
        actor.Init(this);
    }
    public void RemoveActor(Actor actor)
    {
        actors.Remove(actor);
        actor.OnRemove();
    }
    
    public void Start()
    {
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