using System.Drawing;
using Raylib_cs;
using Spritery.Revertable;
using Color = Raylib_cs.Color;

namespace Spritery.Editor.Tools;

public sealed class PencilTool : Tool
{
    private readonly List<Point> _pixels = new();
    private readonly List<Color> _fromColors = new();

    public override string Name => "Pencil";

    public override void Update(float deltaTime, EditorMeta editorMeta)
    {
        var color = editorMeta.Color;

        if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
        {
            editorMeta.UndoList.Do(new PaintAction(_pixels.ToArray(), _fromColors.ToArray(), color));
            
            _pixels.Clear();
            _fromColors.Clear();
        }

        if (!Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT)) return;

        var pixel = editorMeta.HoveredCell;
        var sprite = editorMeta.Sprite;

        var px = pixel.X;
        var py = pixel.Y;

        if (sprite.GetPixel(px, py).Equals(color)) return;

        if (!_pixels.Contains(pixel))
        {
            _pixels.Add(pixel);
            _fromColors.Add(sprite.GetPixel(px, py));
        }

        sprite.SetPixel(px, py, color);
    }
}