using Raylib_cs;
using Spritery.Utils;

namespace Spritery.UI;

public sealed class Label
{
    public Rectangle Bounds { get; set; }
    public string Text { get; set; }

    public LabelProperties Properties { get; set; } = new();

    private int _drawX, _drawY;

    public Label(string text)
    {
        Text = text;
    }

    public Label() : this("")
    {
    }

    public void Draw()
    {
        Raylib.DrawText(Text, _drawX, _drawY, Properties.TextSize, Properties.TextColorNormal);
    }

    public void Update(float deltaTime)
    {
        var textWidth = Raylib.MeasureText(Text, Properties.TextSize);

        if (Properties.HorizontalOrientation == HorizontalOrientation.Left)
        {
            _drawX = (int) (Bounds.Left() + Properties.Padding);
        }
        else if (Properties.HorizontalOrientation == HorizontalOrientation.Center)
        {
            _drawX = (int) (Bounds.x + Bounds.width / 2f - textWidth / 2f);
        }
        else if (Properties.HorizontalOrientation == HorizontalOrientation.Right)
        {
            _drawX = (int) (Bounds.Right() - textWidth - Properties.Padding);
        }

        if (Properties.VerticalOrientation == VerticalOrientation.Top)
        {
            _drawY = (int) (Bounds.Top() + Properties.Padding);
        }
        else if (Properties.VerticalOrientation == VerticalOrientation.Center)
        {
            _drawY = (int) (Bounds.Top() + Bounds.height / 2f - Properties.TextSize / 2f);
        }
        else if (Properties.VerticalOrientation == VerticalOrientation.Bottom)
        {
            _drawY = (int) (Bounds.Bottom() - Properties.TextSize - Properties.Padding);
        }
    }
}

public class LabelProperties : Properties
{
    public HorizontalOrientation HorizontalOrientation { get; set; } = HorizontalOrientation.Left;
    public VerticalOrientation VerticalOrientation { get; set; } = VerticalOrientation.Center;
    public int Padding { get; set; } = 2;
}

public enum VerticalOrientation
{
    Top,
    Center,
    Bottom
}

public enum HorizontalOrientation
{
    Left,
    Center,
    Right
}