using OpenGLGameEngine.ECS;

namespace OpenGLGameEngine.Components;

public class MeshRenderer : Processor<Mesh>
{
    public override void processComponents()
    {
        components.ForEach(mesh =>
        {
            if (mesh.Enabled)
            {
                mesh.Draw();
            }
        });
    }
}