using System.Drawing;
using Color = Raylib_cs.Color;

namespace Spritery.Revertable;

public class PaintAction : RevertableAction
{
    public Point[] Pixels { get; }

    public Color[] FromColors { get; }

    public Color ToColor { get; }

    public PaintAction(Point[] pixels, Color[] fromColors, Color toColor)
    {
        Pixels = pixels;
        FromColors = fromColors;
        ToColor = toColor;
    }

    public override string ToString()
    {
        return $"Paint Action: {Pixels.Length} pixels";
    }
}