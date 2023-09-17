using System.Drawing;
using Raylib_cs;
using Spritery.Revertable;
using Spritery.Utils;
using Color = Raylib_cs.Color;

namespace Spritery.Editor.Tools;

public sealed class EraserTool : Tool
{
    private readonly List<Point> _pixels = new();
    private readonly List<Color> _fromColors = new();

    public override string Name => "Eraser";

    public override void Update(float deltaTime, EditorMeta editorMeta)
    {
        if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
        {
            editorMeta.UndoList.Do(new PaintAction(_pixels.ToArray(), _fromColors.ToArray(), Colors.Invisible));

            _pixels.Clear();
            _fromColors.Clear();
        }

        if (!Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT)) return;

        var pixel = editorMeta.HoveredCell;
        var sprite = editorMeta.Sprite;

        var px = (int) pixel.X;
        var py = (int) pixel.Y;

        if (sprite.GetPixel(px, py).Equals(Colors.Invisible)) return;

        if (!_pixels.Contains(pixel))
        {
            _pixels.Add(pixel);
            _fromColors.Add(sprite.GetPixel(px, py));
        }
        
        sprite.SetPixel(px, py, Colors.Invisible);
    }
}