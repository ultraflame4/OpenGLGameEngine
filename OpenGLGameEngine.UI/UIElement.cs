using System.Drawing;
using System.Numerics;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class UIElement 
{
    public readonly TransformNode transform = new();
    public Vector2 size { get; private set; } = new Vector2(10,10);
    public readonly Mesh backgroundMesh;

    public UIElement()
    {
        backgroundMesh = MeshUtils.CreateQuad(size, color: Color.Gold);
    }

    public void SetSize(Vector2 size)
    {
        this.size = size;
        backgroundMesh.SetVertices(MeshUtils.GetQuadVertices(size));
        backgroundMesh.SetTriangles(0, 2, 1, 1, 2, 3);
    }
    
    public void Render(IRenderCamera camera)
    {
        var shader = MeshRenderer.defaultShader;
        shader.Use();
        shader.SetUniform("model", transform.GetModelMatrix());
        shader.SetUniform("projection", camera.projMatrix);
        shader.SetUniform("view", camera.viewMatrix);
        backgroundMesh.Draw();
    }
}