using OpenGLGameEngine.Components;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Graphics;

namespace OpenGLGameEngine;

public class GameWorld : World
{
    public static Camera? MAIN_CAMERA;

    /// <summary>
    ///     Shader to use globally for rendering unless specified otherwise in the mesh.
    /// </summary>
    public static Shader? GlobalShader = null;

    public Shader? WorldShader;


    public GameWorld()
    {
        WorldShader = GlobalShader;
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