using NLog;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Game;
using OpenGLGameEngine.Graphics;

namespace OpenGLGameEngine.Components;

public class MeshRenderer : Processor<Mesh>
{
    static Logger logger = LogManager.GetCurrentClassLogger();
    public override void processComponents()
    {
        if (GameWorld.MAIN_CAMERA==null)
        {
            logger.Error("No main camera set");
            return;
        }
        if (GameWorld.GlobalShader==null)
        {
            logger.Error("No global shader set!");
            return;
        }
        
        components.ForEach(mesh =>
        {
            if (mesh.Enabled)
            {
                GameWorld.GlobalShader.Use();
                var transformMatrix = mesh.transform.GetModelMatrix() * GameWorld.MAIN_CAMERA.projMatrix;
                GameWorld.GlobalShader.SetUniform("transform", transformMatrix);
                mesh.Draw();
            }
        });
    }
}