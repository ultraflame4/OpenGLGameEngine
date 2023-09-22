using NLog;
using OpenGLGameEngine.Core.Utils;

namespace OpenGLGameEngine.Graphics.Rendering;

public static class RenderPipeline
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    // static List<RenderTarget> renderTargets = new ();
    public static List<IRenderable> renderables { get; } = new();
    public static HashSet<IRenderCamera> cameras { get; } = new();

    public static void Init() { WindowUtils.CheckError(); }

    private static void SetShaderUniforms()
    {
        // todo in future: use uniform buffer objects instead of setting uniforms for every shader
    }

    public static HashSet<string> GetDefaultLayers() { return new HashSet<string>() { "default" }; }

    public static void Render()
    {
        foreach (var cam in cameras)
        {
            cam.renderTarget.Bind();
            cam.renderTarget.Clear();
            foreach (IRenderable renderable in renderables)
            {
                if (!renderable.layers.Overlaps(cam.layers))
                {
                    continue;
                }

                renderable.Render(cam);
            }
        }
    }
}