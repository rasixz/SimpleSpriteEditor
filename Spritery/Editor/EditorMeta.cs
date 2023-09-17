using System.Drawing;
using System.Numerics;
using Spritery.Revertable;
using Color = Raylib_cs.Color;

namespace Spritery.Editor;

public class EditorMeta
{
    public UndoList UndoList { get; set; }
    public Color Color { get; set; }
    public Point HoveredCell { get; set; }
    public Sprite Sprite { get; set; }
}