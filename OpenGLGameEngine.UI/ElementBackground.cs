using System.Drawing;

namespace OpenGLGameEngine.UI;

public class ElementBackground
{
    public Color color;

    public ElementBackground(Color? color = null)
    {
        this.color = color ?? Color.Azure;
    }

    public void Render()
    {
        
    }
}