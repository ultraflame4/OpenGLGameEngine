using OpenGL;

namespace OpenGLGameEngine.Core.Drawing;

public class UniformBlock<T> where T : struct
{
    private readonly uint bindingPoint;
    public readonly uint bufferId;
    public uint size => (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
    
    protected UniformBlock(uint binding_point, T? data = null, BufferUsage usage = BufferUsage.StaticDraw)
    {
        bindingPoint = binding_point;
        bufferId= Gl.GenBuffer();
        Bind();
        Gl.BufferData(BufferTarget.UniformBuffer, size, data, usage);
    }
    
    
    public void SetData(T data, BufferUsage usage = BufferUsage.StaticDraw)
    {
        Bind();
        Gl.BufferData(BufferTarget.UniformBuffer, size, data, usage);
    }
    
    public void Bind()
    {
        Gl.BindBuffer(BufferTarget.UniformBuffer, bufferId);
    }
  
}