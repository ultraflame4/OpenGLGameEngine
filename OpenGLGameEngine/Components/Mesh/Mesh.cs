using NLog;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Game;
using OpenGLGameEngine.Graphics;

namespace OpenGLGameEngine.Components;

public class Mesh : IComponent
{
    public Transform transform;
    public readonly bool TexturesEnabled;

    private float[] _vertices;

    private VertexRenderObject vro;

    public const int TexCoordStride = 2;
    public const int PositionStride = 3;
    public const int ColourStride = 3;

    public Texture? texture;

    /// <summary>
    /// Creates a new mesh
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="enableTextures">Whether to enable the use of textures or not. </param>
    public Mesh(Transform transform, bool enableTextures = false)
    {
        this.transform = transform;
        TexturesEnabled = enableTextures;
        int totalStride = PositionStride + ColourStride + (enableTextures ? TexCoordStride : 0); // Only add texture coords if textures are enabled

        vro = new VertexRenderObject(Array.Empty<float>(), totalStride);
        vro.SetVertexAttrib(0, 3, 0);
        vro.SetVertexAttrib(1, 3, 3);
        // If textures are enabled set the vertex attribute for texture coords
        if (enableTextures)
        {
            vro.SetVertexAttrib(2, 2, 6);
        }
    }

    public void SetVertices(params MeshVertex[] meshVertices)
    {
        _vertices = new float[meshVertices.Length * vro.stride];
        for (var i = 0; i < meshVertices.Length; i++)
        {
            var current = meshVertices[i];
            // Use the properties in the MeshVertex class to set the values in the vertices array because im lazy
            var setter = new MeshVertex(_vertices, i, TexturesEnabled);
            setter.Position = current.Position;
            setter.Color_ = current.Color_;
            setter.TexCoord = current.TexCoord;
        }

        vro.SetVertices(_vertices);
    }

    public MeshVertex GetVertex(int index)
    {
        return new MeshVertex(_vertices, index, TexturesEnabled);
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
    /// Draws the mesh
    /// </summary>
    public void Draw()
    {
        if (TexturesEnabled)
        {
            texture?.Bind();
        }
        vro.Draw();
    }

    public bool Enabled { get; set; } = true;

    public void OnAdd()
    {
        GameWorld.GetInstance().GetProcessor<MeshRenderer>()?.addComponent(this);
    }

    public void OnRemove()
    {
        GameWorld.GetInstance().GetProcessor<MeshRenderer>()?.removeComponent(this);
    }
}