using System.Drawing;
using System.Numerics;
using OpenGLGameEngine.Graphics.Mesh;

namespace OpenGLGameEngine.Actors;

public class PrimitiveShape : MeshRenderer
{
    public static PrimitiveShape CreateCube()
    {
        var actor = new PrimitiveShape();
        actor.Mesh = new Mesh();
        actor.Mesh.SetVertices(
            new MeshVertex(new Vector3(-1f, 1f, 1f), Color.White, new Vector2(0f, 1f)),
            new MeshVertex(new Vector3(1f, 1f, 1f), Color.White, new Vector2(1f, 1f)),
            new MeshVertex(new Vector3(-1f, -1f, 1f), Color.White, new Vector2(0f, 0f)),
            new MeshVertex(new Vector3(1f, -1f, 1f), Color.White, new Vector2(1f, 0f)),
            
            new MeshVertex(new Vector3(-1f, 1f, -1f), Color.White, new Vector2(1f, 1f)),
            new MeshVertex(new Vector3(1f, 1f, -1f), Color.White, new Vector2(0f, 1f)),
            new MeshVertex(new Vector3(-1f, -1f, -1f), Color.White, new Vector2(1f, 0f)),
            new MeshVertex(new Vector3(1f, -1f, -1f), Color.White, new Vector2(0f, 0f))
        );

        actor.Mesh.SetTriangles(
            2, 1, 0,
            1, 2, 3,
            1, 3, 5,
            5, 3, 7,
            4, 5, 7,
            7, 6, 4,
            0, 4, 6,
            6, 2, 0,
            4, 0, 1,
            1, 5, 4,
            6, 7, 2,
            3, 2, 7
        );

        return actor;
    }
}