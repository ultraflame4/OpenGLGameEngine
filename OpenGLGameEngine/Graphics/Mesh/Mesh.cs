using OpenGLGameEngine.Core.Drawing;

namespace OpenGLGameEngine.Graphics.Mesh;

public class Mesh
{
    public const int TexCoordStride = 2;
    public const int PositionStride = 3;
    public const int ColourStride = 3;

    private readonly VertexRenderObject vro;

    private float[] _vertices = Array.Empty<float>();
    public Shader? Shader { get; set; }

    public Texture? texture;


    /// <summary>
    ///     Creates a new mesh
    /// </summary>
    public Mesh( )
    {
        var totalStride = PositionStride + ColourStride + TexCoordStride;

        vro = new VertexRenderObject(Array.Empty<float>(), totalStride);
        vro.AddVertexAttrib( 3);
        vro.AddVertexAttrib( 3);
        vro.AddVertexAttrib(2);
    }


    
    public void SetVertices(params MeshVertex[] meshVertices)
    {
        _vertices = new float[meshVertices.Length * vro.stride];
        for (var i = 0; i < meshVertices.Length; i++)
        {
            var current = meshVertices[i];
            // Use the properties in the MeshVertex class to set the values in the vertices array because im lazy
            var setter = new MeshVertex(_vertices, i);
            setter.Position = current.Position;
            setter.Color_ = current.Color_;
            setter.TexCoord = current.TexCoord;
        }

        vro.SetVertices(_vertices);
    }

    public MeshVertex GetVertex(int index)
    {
        return new MeshVertex(_vertices, index);
    }


    public int GetVertexCount()
    {
        return _vertices.Length / vro.stride;
    }

    public void SetTriangles(params uint[] triangles)
    {
        vro.SetTriangles(triangles);
    }

    public void SetTexture(Texture texture)
    {
        this.texture = texture;
    }

    /// <summary>
    ///     Draws the mesh
    /// </summary>
    public void Draw()
    {
        (texture??Texture.defaultTexture).Bind();
        vro.Draw();
    }
}