using System.Numerics;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class UIElement 
{
    public readonly TransformNode transform = new();
    public Vector2 size { get; private set; } = new Vector2(2,1);
    public readonly Mesh backgroundMesh;

    public UIElement()
    {
        backgroundMesh = MeshUtils.CreateQuad(size);
    }

    public void SetSize(Vector2 size)
    {
        this.size = size;
        backgroundMesh.SetVertices(MeshUtils.GetQuadVertices(size));
    }
    
    public void Render()
    {
        var shader = MeshRenderer.defaultShader;
        shader.Use();
        shader.SetUniform("model", transform.GetModelMatrix());
        shader.SetUniform("projection", Matrix4x4.Identity);
        shader.SetUniform("view", Matrix4x4.Identity);
        backgroundMesh.Draw();
    }
}