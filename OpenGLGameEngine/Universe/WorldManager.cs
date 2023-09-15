namespace OpenGLGameEngine.Universe;

public static class WorldManager
{
    public static World? CurrentWorld { get; private set; } = null;
    
    public static void LoadWorld(World world)
    {
        CurrentWorld?.Remove();
        CurrentWorld = world;
    }

}