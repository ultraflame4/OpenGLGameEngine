using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Components;

public abstract class EntityScript : Component
{
    public bool Enabled { get; set; }
    public override void OnAdd()
    {
        
    }

    public override void OnRemove()
    {
        
    }

    /// <summary>
    /// This method is called when the script 
    /// </summary>
    public abstract void Load();
    public abstract void Start();
    public abstract void Update();
    public abstract void Draw();
}