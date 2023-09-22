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
    public abstract void Destroy();
    public abstract void Render();
}