using NLog;
using OpenGLGameEngine.Core;
using OpenGLGameEngine.Core.Drawing;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.Actors;

public class MeshRenderer : Actor
{
    public static Shader? defaultShader; 
    private Logger logger = LogManager.GetCurrentClassLogger();
    public Mesh Mesh = new ();
    public CameraActor? camera = CameraActor.Main;
    
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
        // var transformMatrix = transform.GetModelMatrix() * camera.viewMatrix * camera.projMatrix;
        // shader.SetUniform("transform", transformMatrix);
        shader.SetUniform("model", transform.GetModelMatrix());
        shader.SetUniform("projection", MainWindow.window.projMatrix);
        shader.SetUniform("view", MainWindow.window.viewMatrix);
        Mesh.Draw();
    }   
}