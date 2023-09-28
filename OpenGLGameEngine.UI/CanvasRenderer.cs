using System.Numerics;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class CanvasRenderer : MeshRenderer
{
    public CanvasRenderer()
    {
        transform.position.Z = -1f;
        Mesh = MeshUtils.CreateQuad(Vector2.One*100);
    }
}