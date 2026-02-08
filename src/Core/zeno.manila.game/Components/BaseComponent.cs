namespace zeno.manila.game.Components;

public abstract class BaseComponent
{
    protected ComponentState ComponentState { get; init; }

    protected BaseComponent(ComponentState componentState)
    {
        ComponentState = componentState;
    }

    public virtual void Update()
    {
        if (ComponentState.ViewportChanged)
        {
            Rearrange();
        }

        foreach (var child in Children)
        {
            child.Update();
        }
    }

    protected virtual void Rearrange()
    {

    }

    public virtual void Draw()
    {
        foreach (var child in Children)
        {
            child.Draw();
        }
    }

    public virtual void UpdateContainedBounds()
    {
        WithinBounds = false;
        var mouse = ComponentState.InputManager.GetMousePosition();

        if (X + Margin <= mouse.X && mouse.X <= X + Width &&
            Y + Margin <= mouse.Y && mouse.Y <= Y + Height)
        {
            WithinBounds = true;
            foreach (var child in Children)
            {
                child.UpdateContainedBounds();
            }
        }
    }

    public Rectangle Bounds => new(X + Margin, Y + Margin, Width, Height);

    public int Margin { get; set; } = 8;
    public int Padding { get; set; } = 8;
    public int Width { get; set; }
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsActive { get; set; } = true;
    public bool WithinBounds { get; set; }
    public IList<BaseComponent> Children { get; set; } = [];
    public int GetChildIndex()
    {
        if (Parent is null)
        {
            return -1;
        }

        return Parent.Children.IndexOf(this);
    }
    public BaseComponent? Parent { get; set; }
    public BaseComponent? Root
    {
        get
        {
            BaseComponent? root = Parent;

            while (root?.Parent is not null)
            {
                root = root?.Parent;
            }

            return root;
        }
    }
}
