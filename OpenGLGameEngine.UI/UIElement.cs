using System.Numerics;
using OpenGLGameEngine.Actors;

namespace OpenGLGameEngine.UI;

public class UIElement : MeshRenderer
{
    
    Vector2 size = Vector2.One * 100;

    public UIElement()
    {
        layers = new HashSet<string> { "ui" };
    }
}