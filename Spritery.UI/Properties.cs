using Raylib_cs;
using Spritery.Utils;

namespace Spritery.UI;

public class Properties
{
    public Color BorderColorNormal { get; set; } = Colors.FromRgba(0x838383ff);
    public Color BaseColorNormal { get; set; } = Colors.FromRgba(0xc9c9c9ff);
    public Color TextColorNormal { get; set; } = Colors.FromRgba(0x686868ff);
    public Color BorderColorFocused { get; set; } = Colors.FromRgba(0x5bb2d9ff);
    public Color BaseColorFocused { get; set; } = Colors.FromRgba(0xc9effeff);
    public Color TextColorFocused { get; set; } = Colors.FromRgba(0x6c9bbcff);
    public Color BorderColorPressed { get; set; } = Colors.FromRgba(0x0492c7ff);
    public Color BaseColorPressed { get; set; } = Colors.FromRgba(0x97e8ffff);
    public Color TextColorPressed { get; set; } = Colors.FromRgba(0x368bafff);
    public Color BorderColorDisabled { get; set; } = Colors.FromRgba(0xb5c1c2ff);
    public Color BaseColorDisabled { get; set; } = Colors.FromRgba(0xe6e9e9ff);
    public Color TextColorDisabled { get; set; } = Colors.FromRgba(0xaeb7b8ff);

    public int BorderWidth { get; set; } = 1;
    public int TextSize { get; set; } = 10;

    public Color LineColor { get; set; } = Colors.FromRgba(0x90abb5ff);
    public Color BackgroundColor { get; set; } = Colors.FromRgba(0xf5f5f5ff);
}