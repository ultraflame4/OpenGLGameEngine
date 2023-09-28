using System.Drawing;
using System.Numerics;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Math;

namespace GameEngineTest;


public class TestObject : MeshRenderer
{
    private Texture renderTex;
    public TestObject(Texture renderTex) { this.renderTex = renderTex; }

    public override void Start()
    {
        transform.position.X = 2f;
        Mesh = new Mesh();
        Mesh.SetVertices(
            new MeshVertex(new Vector3(-1f, 1f, 0f), Color.White, new Vector2(0f, 1f)),
            new MeshVertex(new Vector3(1f, 1f, 0f), Color.White, new Vector2(1f, 1f)),
            new MeshVertex(new Vector3(1f, -1f, 0f), Color.White, new Vector2(1f, 0f)),
            new MeshVertex(new Vector3(-1f, -1f, 0f), Color.White, new Vector2(0f, 0f))
        );

        Mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        Mesh.SetTexture(renderTex);

        // transform.scale = new Vector3(0.5f);
    }

    public override void DrawTick()
    {
        transform.rotation.RotateEuler(Vector3.UnitZ*GameTime.DeltaTime);
    }
}