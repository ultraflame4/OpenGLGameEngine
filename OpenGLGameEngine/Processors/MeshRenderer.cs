using NLog;
using OpenGLGameEngine.Components.Mesh;
using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Processors;

public class MeshRenderer : Processor<Mesh>
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public override void processComponents()
    {
        if (GameWorld.MAIN_CAMERA == null)
        {
            logger.Error("No main camera set");
            return;
        }

        if (GameWorld.GlobalShader == null)
        {
            logger.Error("No global shader set!");
            return;
        }

        components.ForEach(mesh =>
        {
            if (mesh.Enabled)
            {
                var shader = mesh.Shader ?? GameWorld.GlobalShader;
                shader.Use();
                var transformMatrix = mesh.transform.GetModelMatrix() * GameWorld.MAIN_CAMERA.projMatrix;
                shader.SetUniform("transform", transformMatrix);
                mesh.Draw();
            }
        });
    }
}