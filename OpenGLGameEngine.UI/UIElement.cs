namespace OpenGLGameEngine.UI;

public abstract class UIElement
{
    private UIElement? parent { get; set; } = null;
    private List<UIElement> children { get; } = new();

    public void AddChild(UIElement child)
    {
        child.parent = this;
        children.Add(child);
    }
    public void RemoveChild(UIElement child)
    {
        child.parent = null;
        children.Remove(child);
        child.Destroy();
    }

    public virtual void Update() { }

    public virtual void Destroy()
    {
        // Remove all children in list.
        // We can modify the list while iterating over it because we're iterating backwards.
        for (var i = children.Count - 1; i >= 0; i--)
        {
            RemoveChild(children[i]);
        }
    }

    public virtual void Render()
    {
        children.ForEach(child => child.Render());
    }
}