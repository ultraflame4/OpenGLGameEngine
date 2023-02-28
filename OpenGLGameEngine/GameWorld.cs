using OpenGLGameEngine.Components;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Graphics;

namespace OpenGLGameEngine;

public class GameWorld : World
{
    public static Camera? MAIN_CAMERA = null;
    private static GameWorld instance;
    /// <summary>
    /// Shader to use globally for rendering unless specified otherwise in the mesh.
    /// </summary>
    public static Shader? GlobalShader=null;
    
    public static GameWorld GetInstance()
    {
        if (instance == null)
        {
            instance = new GameWorld();
        }

        return instance;
    }


    public Entity CreateMainCamera()
    {
        var entity = CreateEntity();
        entity.AddComponent(new Transform());
        entity.AddComponent(new Camera());
        MAIN_CAMERA = entity.GetComponent<Camera>();
        return entity;
    }

}