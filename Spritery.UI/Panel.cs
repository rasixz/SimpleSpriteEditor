using Raylib_cs;
using Spritery.Utils;

namespace Spritery.UI;

public class Panel
{
    public Rectangle Bounds { get; set; }
    public int Padding { get; set; } = 5;
    public string? Title { get; set; }

    public Rectangle ContentBounds { get; private set; }
    protected Rectangle TitleBounds;

    public PanelProperties Properties { get; set; } = new();

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(Bounds, Properties.BackgroundColor);
        Raylib.DrawRectangleLinesEx(Bounds, Properties.BorderWidth, Properties.BorderColorNormal);

        if (Title != null)
        {
            Raylib.DrawRectangleLinesEx(TitleBounds, Properties.BorderWidth, Properties.BorderColorNormal);
            Raylib.DrawText(Title, (int) (TitleBounds.x + TitleBounds.width / 2f - Raylib.MeasureText(Title, Properties.TextSize) / 2f), (int) (TitleBounds.y + TitleBounds.height / 2f - Properties.TextSize / 2f), Properties.TextSize, Properties.TextColorNormal);
        }
    }

    public virtual void Update(float deltaTime)
    {
        if (Title == null)
        {
            ContentBounds = new Rectangle(Bounds.x + Padding, Bounds.y + Padding, Bounds.width - 2 * Padding, Bounds.height - 2 * Padding);
        }
        else
        {
            TitleBounds = new Rectangle(Bounds.x, Bounds.y, Bounds.width, Properties.TitleBarHeight);
            ContentBounds = new Rectangle(Bounds.x + Padding, TitleBounds.Bottom() - 1 + Padding, Bounds.width - 2 * Padding, Bounds.height - TitleBounds.height + 1 - 2 * Padding);
        }
    }
}

public class PanelProperties : Properties
{
    public int TitleBarHeight = 20;
}