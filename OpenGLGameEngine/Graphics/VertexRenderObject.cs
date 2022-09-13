﻿using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using NLog;
using OpenGL;

namespace OpenGLGameEngine.Graphics;

/// <summary>
/// A light wrapper around VBOs and VAOs to abstract away some complexities and make them less annoying.
/// </summary>
public class VertexRenderObject
{
    public uint VBO;
    public uint VAO;
    public uint? EBO = null;
    public int stride;
    static Logger logger = LogManager.GetCurrentClassLogger();
    private int draw_count = 0;

    ///  <summary>
    ///  <b>BufferUsage Guide:</b>
    ///  <br/>
    ///  STREAM
    ///  <br/>
    ///  You should use STREAM_DRAW when the data store contents will be modified once and used at most a few times.
    ///  <br/>
    /// STATIC
    ///  <br/>
    ///  Use STATIC_DRAW when the data store contents will be modified once and used many times.
    ///  <br/>
    ///  DYNAMIC
    ///  <br/>
    ///  Use DYNAMIC_DRAW when the data store contents will be modified repeatedly and used many times.
    ///  <br/>
    ///  Stride example: if vertices is [x1,y1,x2,y2], stride is 2, [x1,y1,z1,x2,y2,z2] = stride is 3
    ///  </summary>
    ///  <param name="vertices">A float array of vertices. </param>
    ///  <param name="stride">How long it takes before it reaches the next set of attributes</param>
    ///  <param name="indices">Array of vertex index in vertices array. Used to reduce duplicate vertices.</param>
    ///  <param name="usage">How the internal vertex buffer will be used.</param>
    public VertexRenderObject(
        float[] vertices,
        int stride,
        uint[]? indices = null,
        BufferUsage usage = BufferUsage.StaticDraw)
    {
        this.stride = stride;
        VBO = Gl.GenBuffer();
        VAO = Gl.GenVertexArray();


        Bind();
        
        Gl.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices, usage);
        
        if (indices is not null)
        {
            EBO = Gl.GenBuffer();
            draw_count = indices.Length;
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, (uint)EBO);
            Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)(indices.Length * sizeof(int)), indices, usage);
            logger.Debug($"Created EBO because indices is not null. EBO {EBO} with draw count {draw_count}");
        }
        else
        {
            draw_count = vertices.Length / stride;
        }


        logger.Debug($"Created vao ({VAO}) and VBO ({VBO}) with draw count: {draw_count}");
    }

    /// <summary>
    /// Sets an vertex attribute in opengl.
    /// <br/>
    /// The data type,normalized is automatically set.
    /// <code>
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
    /// <param name="index">The index of this attribute</param>
    /// <param name="size">The size of the attribute. (in number of items not bytes)</param>
    /// <param name="offset_index">Offset</param>
    /// <param name="enable">Whether to enable the vertex attriute</param>
    public void SetVertexAttrib(uint index, int size, int offset_index, bool enable = true,
        [CallerFilePath] string callerFile = "", [CallerLineNumber] int lineno = -1)
    {
        Gl.GetInteger(GetPName.VertexArrayBinding, out int index_);
        if (VAO != index_)
        {
            logger.Warn($"{callerFile}(\n{lineno}) !! Current Bound Vertex Attribute Object (VAO) (current:{index_}, instance{index}) is not the one in this instance!");
            logger.Warn($"Will AutoBind!");
            Bind();
        }

        Gl.VertexAttribPointer(index, size, VertexAttribType.Float, false, stride * sizeof(float), (IntPtr)(offset_index * sizeof(float)));
        if (enable)
        {
            Gl.EnableVertexAttribArray(index);
        }
    }

    public void Bind()
    {
        Gl.BindVertexArray(VAO);
    }

    public void Draw()
    {
        Bind();
        if (EBO is null)
        {
            Gl.DrawArrays(PrimitiveType.Triangles, 0, draw_count);
        }
        else
        {
            Gl.DrawElements(PrimitiveType.Triangles, draw_count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}