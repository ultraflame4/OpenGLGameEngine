using System.Numerics;
using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class ElementRectTransform : TransformNode
{
    public Vector3 size;

    public Vector3 topLeft
    {
        get => new Vector3(position.X - size.X / 2, position.Y + size.Y / 2,0);
        set => position = new Vector3(value.X + size.X / 2, value.Y - size.Y / 2,0);
    }

    public Vector3 topRight
    {
        get => position + size / 2;
        set => position = value - size / 2;
    }

    public Vector3 bottomLeft
    {
        get => position - size / 2;
        set => position = value + size / 2;
    }

    public Vector3 bottomRight
    {
        get => new Vector3(position.X + size.X / 2, position.Y - size.Y / 2,0);
        set => position = new Vector3(value.X - size.X / 2, value.Y + size.Y / 2,0);
    }
}