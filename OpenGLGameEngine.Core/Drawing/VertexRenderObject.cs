using System.Runtime.CompilerServices;
using NLog;
using OpenGL;

namespace OpenGLGameEngine.Core.Drawing;

/// <summary>
///     A light wrapper around VBOs and VAOs to ease rendering of vertices and triangles.
///     <br />
///     <br />
///     Essentially, we store an array of items, each item has attribute like position and color
///     <br />
///     For example:
///     <code>
/// [x,y,z, r,g,b, x2,y2,z2, r2,g2,b2, ...]
/// </code>
///     What we have here is an interlaced array with data describing different things, which are, 1. position(s), 2. color(s)
///     We first separate the different "items" mixed together
///     <code>
/// [ (x,y,z, r,g,b), (x2,y2,z2, r2,g2,b2), ...]
/// </code>
///     Now we can see the different items. <br />
///     Then we now separate the different attributes of the items
///     <code>
/// [ ( {x,y,z}, {r,g,b}), ({x2,y2,z2}, {r2,g2,b2}), ...]
/// </code>
///     This is essentially how we get around not being able to use classes to nicely classify the data
/// </summary>
public class VertexRenderObject
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly string callerFile;
    private readonly int lineno;
    private readonly BufferUsage bufferUsage;
    private int draw_count; // number of triangles to draw
    public uint? ElementBufferObject;
    public int stride;
    public uint VertexArrayObject;
    public uint VertexBufferObject;

    private uint _attrib_index_counter;
    private uint _attrib_offset_counter;

    /// <summary>
    ///     <b>BufferUsage Guide:</b>
    ///     <br />
    ///     STREAM
    ///     <br />
    ///     You should use STREAM_DRAW when the data store contents will be modified once and used at most a few times.
    ///     <br />
    ///     STATIC
    ///     <br />
    ///     Use STATIC_DRAW when the data store contents will be modified once and used many times.
    ///     <br />
    ///     DYNAMIC
    ///     <br />
    ///     Use DYNAMIC_DRAW when the data store contents will be modified repeatedly and used many times.
    ///     <br />
    ///     Stride example: if vertices is [x1,y1,x2,y2], stride is 2, [x1,y1,z1,x2,y2,z2] = stride is 3
    /// </summary>
    /// <param name="vertices">A float array of vertices. </param>
    /// <param name="stride">How long it takes before it reaches the next set of attributes</param>
    /// <param name="indices">Array of vertex index in vertices array. Used to reduce duplicate vertices.</param>
    /// <param name="usage">How the internal vertex buffer will be used.</param>
    public VertexRenderObject(
        float[] vertices,
        int stride,
        uint[]? indices = null,
        BufferUsage usage = BufferUsage.StaticDraw,
        [CallerFilePath] string callerFile = "", [CallerLineNumber] int lineno = -1
    )
    {
        this.stride = stride;
        this.callerFile = callerFile;
        this.lineno = lineno;
        bufferUsage = usage;

        VertexBufferObject = Gl.GenBuffer();
        VertexArrayObject = Gl.GenVertexArray();

        SetVertices(vertices);
        if (indices is not null) SetTriangles(indices);
    }

    public void SetVertices(float[] vertices)
    {
        Bind();
        Gl.BindBuffer(BufferTarget.ArrayBuffer,
            VertexBufferObject); // Bind the VBO to the VAO and so we can put data into the buffer
        Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices,
            bufferUsage); // put in data
        draw_count = vertices.Length / stride;
    }

    public void SetTriangles(uint[] indices)
    {
        Bind();
        if (ElementBufferObject is null)
            ElementBufferObject = Gl.GenBuffer(); // If we have not created an EBO, create one

        draw_count = indices.Length; // number of triangles to draw
        Gl.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)ElementBufferObject); // bind the EBO to the VAO
        Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)(indices.Length * sizeof(uint)), indices,
            bufferUsage); // do some config for EBO
    }

    /// <summary>
    ///     Sets an vertex attribute in opengl.
    ///     <br />
    ///     The data type,normalized is automatically set.
    ///     <code>
    /// Example representation:
    /// -
    /// tx,ty = texture coordinates
    /// x,y,z = vertex positions
    /// -
    /// Vertices = [x1,y1,z1,tx1,ty1, x2,y2,z2,tx2,ty2]
    /// .
    /// Position attributes "group" wld be all the x,y,z. Because there are 3 items, x,y,z The size would be '3'
    /// .
    /// Texture coords attributes "group" wld be all tx,ty. Because 2 items, size is 2
    /// .
    /// Stride is 6 because it takes 6 items before it reaches the next set of attributes.
    /// .
    /// Offset for position is 0, because it is at the start
    /// Offset for texture coords is 3, because that is its index.
    /// </code>
    /// </summary>
    /// <param name="size">The size of the attribute. (in number of items not bytes)</param>
    /// <param name="enable">Whether to enable the vertex attribute</param>
    public void AddVertexAttrib(
        int size, bool enable = true,
        [CallerFilePath] string callerFile = "",
        [CallerLineNumber] int lineno = -1)
    {
        Gl.GetInteger(GetPName.VertexArrayBinding, out int index_);
        if (VertexArrayObject != index_)
        {
            logger.Warn(
                $"{callerFile}({lineno}) !!" +
                $" Current Bound Vertex Attribute Object (VAO) (current:{index_}, " +
                $"instance {VertexArrayObject}) is not the one in this instance!");
            logger.Warn("Will AutoBind!");
            Bind();
        }

        Gl.VertexAttribPointer(
            _attrib_index_counter,
            size,
            VertexAttribType.Float,
            false,
            stride * sizeof(float),
            (IntPtr)(_attrib_offset_counter * sizeof(float)));
        
        if (enable) Gl.EnableVertexAttribArray(_attrib_index_counter);

        _attrib_index_counter++;
        _attrib_offset_counter += (uint)size;
    }

    public void Bind()
    {
        Gl.BindVertexArray(VertexArrayObject);
    }

    public void Draw()
    {
        Bind();
        if (ElementBufferObject is null)
            Gl.DrawArrays(PrimitiveType.Triangles, 0, draw_count);
        else
            Gl.DrawElements(PrimitiveType.Triangles, draw_count, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    ~VertexRenderObject()
    {
        logger.Warn(
            $"{callerFile}({lineno})!!! Vertex Render Object is being deleted! This will cause memory leaks!!! Please change the contents of the buffer instead!");
    }
}