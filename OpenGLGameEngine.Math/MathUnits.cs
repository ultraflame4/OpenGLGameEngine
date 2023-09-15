namespace OpenGLGameEngine.Math;

public static class MathUnits
{
    public static float ToRad(this float degrees)
    {
        return (MathF.PI / 180f) * degrees;
    }

    public static float ToDeg(this float radians)
    {
        return (180f / MathF.PI) * radians;
    }
    
}