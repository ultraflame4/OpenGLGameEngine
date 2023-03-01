using OpenGLGameEngine.Core.Graphics;
using OpenGLGameEngine.ECS;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Processors;

namespace OpenGLGameEngine.Components.Mesh;

public class Mesh : Component
{
    public const int TexCoordStride = 2;
    public const int PositionStride = 3;
    public const int ColourStride = 3;
    public readonly bool TexturesEnabled;

    private readonly VertexRenderObject vro;

    private float[] _vertices;

    private Shader? shader;

    public Texture? texture;
    public Transform transform;

    /// <summary>
    ///     Creates a new mesh
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="enableTextures">Whether to enable the use of textures or not. </param>
    public Mesh(Transform transform, bool enableTextures = false)
    {
        this.transform = transform;
        TexturesEnabled = enableTextures;
        var totalStride = PositionStride + ColourStride + (enableTextures ? TexCoordStride : 0); // Only add texture coords if textures are enabled

        vro = new VertexRenderObject(Array.Empty<float>(), totalStride);
        vro.SetVertexAttrib(0, 3, 0);
        vro.SetVertexAttrib(1, 3, 3);
        // If textures are enabled set the vertex attribute for texture coords
        if (enableTextures) vro.SetVertexAttrib(2, 2, 6);
    }

    public Shader? Shader
    {
        get => (shader ?? World.ToGameWorld()?.WorldShader) ?? GameWorld.GlobalShader;
        set => shader = value;
    }

    public bool Enabled { get; set; } = true;

    public override void OnAdd()
    {
        logger.Info("Adding mesh to world!. Current World Type: {worldType}", World?.GetType());
        World?.GetProcessor<MeshRenderer>()?.addComponent(this);
    }

    public override void OnRemove()
    {
        World?.GetProcessor<MeshRenderer>()?.removeComponent(this);
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
    ///     Draws the mesh
    /// </summary>
    public void Draw()
    {
        if (TexturesEnabled) texture?.Bind();
        vro.Draw();
    }
}