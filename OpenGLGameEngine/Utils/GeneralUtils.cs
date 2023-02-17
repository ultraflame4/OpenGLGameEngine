using System.Numerics;

namespace OpenGLGameEngine.Utils;

/// <summary>
/// A general utility class that has useful functions
/// </summary>
public static class GeneralUtils
{
    /// <summary>
    /// Returns the center position of an item using its top left position and size
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public static (int x, int y) TopLeft2CenterPosition(int x, int y, int w, int h)
    {
        int cx = (int)MathF.Round(x + w / 2);
        int cy = (int)MathF.Round(y + h / 2);
        return (cx, cy);
    }

    /// <summary>
    /// Converts a position (or any tuple with 2 integers) into a vector2
    /// </summary>
    /// <param name="xy"></param>
    /// <returns></returns>
    public static Vector2 xy2Vector2(this (int x, int y) xy)
    {
        return new Vector2(xy.x, xy.y);
    }

    /// <summary>
    /// Converts a Matrix4x4 to an array
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static float[] Mat4x4ToArray(this Matrix4x4 mat)
    {
        return new float[] {
                mat.M11, mat.M12, mat.M13, mat.M14,
                mat.M21, mat.M22, mat.M23, mat.M24,
                mat.M31, mat.M32, mat.M32, mat.M34,
                mat.M41, mat.M42, mat.M42, mat.M44,
        };
    }
}