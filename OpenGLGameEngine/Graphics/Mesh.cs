namespace OpenGLGameEngine.Graphics;

public class Mesh
{
    private float[] bufferData; // data to store like position, color, texture
    public const int VertexStride = 3; 
    public const int ColorStride = 3; 
    public const int TexCoordsStride = 2;

    public int Stride => VertexStride + ColorStride + TexCoordsStride;
    
    public Mesh()
    {
        
    }
    
    
}