using System.Drawing;
using OpenGL;
using OpenGLGameEngine.Core.Utils;
using OpenGLGameEngine.Graphics.LowLevel;

namespace OpenGLGameEngine.UI;

public class ElementBackground
{
    public static Shader? backgroundShader;
    
    public Color color;

    private static bool initialized = false;
    public static void InitShader()
    {
        if (initialized) return;
        backgroundShader = ShaderManager.instance.CreateProgram(
            ShaderManager.instance.defaultVertexShader,
            ShaderManager.instance.SingleFromResource("OpenGLGameEngine.UI.Resources.Shaders.solid.fragment.glsl")
        );
    }
    
    public ElementBackground(Color? color = null)
    {
        this.color = color ?? Color.Azure;
    }

    public void Render(ElementRectTransform rect)
    {
        
    }
}