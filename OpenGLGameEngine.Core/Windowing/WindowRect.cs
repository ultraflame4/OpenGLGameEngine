using OpenGLGameEngine.Math;

namespace OpenGLGameEngine.Core.Windowing;

public struct WindowRect
{
    public Point position = new(0, 0);
    public Point size = new(0, 0);

    public float aspectRatio => size.X / (float)size.Y;
    public int Width => size.X;
    public int Height => size.Y;
    public int X => position.X;
    public int Y => position.Y;
    

    public WindowRect(int x, int y, int w, int h)
    {
        this.position = new Point(x, y);
        this.size = new Point(w, h);
    }

    public WindowRect(Point position, Point size)
    {
        this.position = position;
        this.size = size;
    }

    public override string ToString() { return $"<{GetType().FullName} Position: {position}; Size: {size}>"; }

    /// <summary>
    /// Returns the center of the window rect
    /// </summary>
    public Point center => new(X + Width / 2, Y + Height / 2);
}