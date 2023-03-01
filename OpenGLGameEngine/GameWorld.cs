using OpenGLGameEngine.Components;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Processors;

namespace OpenGLGameEngine;

public class GameWorld : World
{
    public static Camera? MAIN_CAMERA;

    /// <summary>
    ///  Shader to use globally for rendering unless specified otherwise in the mesh.
    /// </summary>
    public static Shader? GlobalShader;

    public Shader? WorldShader;

    public readonly MeshRenderer MeshRenderer;
    public readonly EntityScriptExecutor EntityScriptExecutor;
    
    public GameWorld()
    {
        WorldShader = GlobalShader;
        MeshRenderer = new MeshRenderer();
        EntityScriptExecutor = new EntityScriptExecutor();
        AddProcessor(MeshRenderer);
        AddProcessor(EntityScriptExecutor);
    }

    public Entity CreateMainCamera()
    {
        var entity = CreateEntity();
        entity.AddComponent(new Transform());
        entity.AddComponent(new Camera());
        MAIN_CAMERA = entity.GetComponent<Camera>();
        return entity;
    }
    
    public EntityObject AddEntityObject(EntityObject entityObject)
    {
        var entity = CreateEntity();
        var transform = entity.AddComponent(new Transform());
        entityObject.transform = transform;
        entity.AddComponent(entityObject);
        return entityObject;
    }
    
    
}