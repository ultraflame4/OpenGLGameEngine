using NLog;
using OpenGLGameEngine.Core.Graphics;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.Components;

public class MeshRenderer : Actor
{
    public static Shader? defaultShader; 
    private Logger logger = LogManager.GetCurrentClassLogger();
    public Mesh.Mesh Mesh = new ();
    public CameraActor? camera = CameraActor.main;
    
    public override void DrawTick()
    {
        if (camera == null)
        {
            logger.Error("Camera is null! Cannot render mesh!");
            return;
        }
        var shader = Mesh.Shader ?? defaultShader;
        if (shader == null)
        {
            logger.Error("No shader! Cannot render mesh!");
            return;
        }
        shader.Use();
        var transformMatrix = transform.GetModelMatrix() * camera.viewMatrix * camera.projMatrix;
        shader.SetUniform("transform", transformMatrix);
        Mesh.Draw();
    }   
}