using System.Drawing;
using System.Numerics;
using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Graphics.Mesh;

public static class MeshUtils
{
    public static Mesh CreateQuad(Vector2? size = null, Vector2? offset = null, Color? color = null)
    {
        Vector2 half = (size ?? Vector2.One) / 2;
        Color color_ = color ?? Color.White;
        Vector3 offset_ = (offset ?? Vector2.Zero).toVec3();
        var m = new Mesh();
        m.SetVertices(
            new MeshVertex(new Vector3(-half.X, half.Y, 0) + offset_, color_, Vector2.UnitY),
            new MeshVertex(new Vector3(half.X, half.Y, 0) + offset_, color_, Vector2.One),
            new MeshVertex(new Vector3(-half.X, -half.Y, 0) + offset_, color_, Vector2.Zero),
            new MeshVertex(new Vector3(half.X, -half.Y, 0) + offset_, color_, Vector2.UnitX)
        );
        m.SetTriangles(0,2 ,1, 1, 2, 3);
        return m;
    }
}