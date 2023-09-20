using NLog;

namespace OpenGLGameEngine.Graphics.Rendering;

public static class RenderPipeline
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    // static List<RenderTarget> renderTargets = new ();
    public static List<IRenderable> renderables { get; } = new();
    public static HashSet<IRenderCamera> cameras { get; } = new();
    public static void Init() { }

    private static void SetShaderUniforms()
    {
        // todo in future: use uniform buffer objects instead of setting uniforms for every shader
    }

    public static void Render()
    {
        foreach (var cam in cameras)
        {
            cam.renderTarget.Bind();
            cam.renderTarget.Clear();
            foreach (IRenderable renderable in renderables)
            {
                renderable.Render(cam);
            }
        }
    }
}