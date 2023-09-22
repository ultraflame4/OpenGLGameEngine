using OpenGLGameEngine.Universe;

namespace OpenGLGameEngine.UI;

public class UIElement : TransformNode
{
    private UIElement? parent { get; set; } = null;
    private List<UIElement> children { get; } = new();

    public int width = 100;
    public int height = 100;
    public ElementBackground Background { get; } = new();


    public virtual void Update() { }

    /// <summary>
    /// Removes a child from this node and destroys it.
    /// </summary>
    /// <param name="child"></param>
    public void DestroyChild(TransformNode child)
    {
        RemoveChild(child);
        child.Destroy();
    }

    /// <summary>
    /// Destroys this element and all its children.
    /// </summary>
    public override void Destroy()
    {
        // Remove all children in list.
        // We can modify the list while iterating over it because we're iterating backwards.
        for (var i = children.Count - 1; i >= 0; i--)
        {
            DestroyChild(children[i]);
        }
    }

    public virtual void Render()
    {
        Background.Render();
        children.ForEach(child => child.Render());
    }
}