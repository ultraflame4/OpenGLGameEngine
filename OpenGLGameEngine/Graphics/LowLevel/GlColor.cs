using System.Drawing;

namespace OpenGLGameEngine.Graphics.LowLevel;

public struct GlColor
{
    public float r;
    public float g;
    public float b;
    public float a;

    public static GlColor FromColor(Color color)
    {
        return new GlColor() {
                r = color.R / 255f,
                g = color.G / 255f,
                b = color.B / 255f,
                a = color.A / 255f
        };
    }
}