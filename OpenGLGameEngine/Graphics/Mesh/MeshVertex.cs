using System.Drawing;
using System.Numerics;

namespace OpenGLGameEngine.Graphics.Mesh;

/// <summary>
/// Utility class to make accessing mesh vertices easier and hides behind the terrifying single array non-OOP mess.
/// Represents a single vertex in a mesh.
/// </summary>
public class MeshVertex
{
    private readonly float[] vertices;
    public int vertexIndex;

    /// <summary>
    ///     This constructor is used when you want to access a vertex that is already in the vertices array
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="vertexIndex"></param>
    /// <param name="texturesEnabled"></param>
    public MeshVertex(float[] vertices, int vertexIndex)
    {
        var stride = 8 ;
        this.vertices = vertices;
        this.vertexIndex = vertexIndex * stride;
    }

    /// <summary>
    ///     This one is used when the user is creating an new vertices array.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="colour"></param>
    /// <param name="texCoord"></param>
    public MeshVertex(Vector3 position, Color colour, Vector2 texCoord)
    {
        vertices = new float[8];
        vertexIndex = 0;
        Position = position;
        Color_ = colour;
        TexCoord = texCoord;
    }

    public float X
    {
        get => vertices[vertexIndex];
        set => vertices[vertexIndex] = value;
    }

    public float Y
    {
        get => vertices[vertexIndex + 1];
        set => vertices[vertexIndex + 1] = value;
    }

    public float Z
    {
        get => vertices[vertexIndex + 2];
        set => vertices[vertexIndex + 2] = value;
    }

    public float R
    {
        get => vertices[vertexIndex + 3];
        set => vertices[vertexIndex + 3] = value;
    }

    public float G
    {
        get => vertices[vertexIndex + 4];
        set => vertices[vertexIndex + 4] = value;
    }

    public float B
    {
        get => vertices[vertexIndex + 5];
        set => vertices[vertexIndex + 5] = value;
    }

    public float TexCoordX
    {
        get => vertices[vertexIndex + 6];
        set { vertices[vertexIndex + 6] = value; }
    }

    public float TexCoordY
    {
        get => vertices[vertexIndex + 7];
        set { vertices[vertexIndex + 7] = value; }
    }

    public Vector3 Position
    {
        get => new(X, Y, Z);
        set
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }
    }

    public Color Color_
    {
        get => Color.FromArgb((int)(R * 255), (int)(G * 255), (int)(B * 255));
        set
        {
            R = (float)value.R / 255;
            G = (float)value.G / 255;
            B = (float)value.B / 255;
        }
    }

    public Vector2 TexCoord
    {
        get => new(TexCoordX, TexCoordY);
        set
        {
            TexCoordX = value.X;
            TexCoordY = value.Y;
        }
    }
}