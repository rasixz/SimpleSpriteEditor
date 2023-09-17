using Raylib_cs;
using Spritery.UI;
using Spritery.Utils;

namespace Spritery.Editor;

public sealed class ColorPalettePanel : Panel
{
    private readonly List<ColorPalette> _colorPalettes = new();
    private int _selectedColorPalette = 0;

    public ColorPalette SelectedPalette => _colorPalettes.Count == 0 ? null : _colorPalettes[_selectedColorPalette];
    public Color SelectedColor => _colorsPanel.SelectedColor;

    private readonly Label _paletteNameLabel;
    private readonly Button _loadPaletteButton;
    private readonly Button _nextPaletteButton;
    private readonly Button _prevPaletteButton;
    
    private readonly PalettePanel _colorsPanel;

    public ColorPalettePanel()
    {
        Title = "Color Palette";
        _paletteNameLabel = new Label {Properties = {HorizontalOrientation = HorizontalOrientation.Center}};
        _loadPaletteButton = new Button("Load Palette from File...") {Enabled = false};
        _nextPaletteButton = new Button("Next");
        _nextPaletteButton.ButtonClickedEvent += (_, _) =>
        {
            if (_selectedColorPalette < _colorPalettes.Count)
                _selectedColorPalette++;
        };
        _prevPaletteButton = new Button("Previous");
        _prevPaletteButton.ButtonClickedEvent += (_, _) =>
        {
            if (_selectedColorPalette > 0)
                _selectedColorPalette--;
        };
        _colorsPanel = new PalettePanel();
        
        LoadAllPalettes();
    }

    private void LoadAllPalettes()
    {
        var files = Directory.GetFiles("resources/palettes","*.txt");
        foreach (var file in files)
        {
            var palette = ColorPalette.FromPaintNetTxtFile(file);
            _colorPalettes.Add(palette);
        }
    }

    public override void Draw()
    {
        base.Draw();

        _paletteNameLabel.Draw();
        _loadPaletteButton.Draw();
        _nextPaletteButton.Draw();
        _prevPaletteButton.Draw();
        _colorsPanel.Draw();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        _paletteNameLabel.Bounds = Rectangles.FromLtrb(ContentBounds.Left(), ContentBounds.Top(), ContentBounds.Right(), ContentBounds.Top() + Properties.TextSize * 1.5f);
        _loadPaletteButton.Bounds = Rectangles.FromLtrb(ContentBounds.Left(), _paletteNameLabel.Bounds.Bottom() + Padding, ContentBounds.Right(), _paletteNameLabel.Bounds.Bottom() + 25);
        _prevPaletteButton.Bounds = Rectangles.FromLtrb(ContentBounds.Left(), _loadPaletteButton.Bounds.Bottom() + Padding, ContentBounds.Right() - ContentBounds.width / 2f - Padding / 2f, _loadPaletteButton.Bounds.Bottom() + 25);
        _nextPaletteButton.Bounds = Rectangles.FromLtrb(_prevPaletteButton.Bounds.Right() + Padding, _loadPaletteButton.Bounds.Bottom() + Padding, ContentBounds.Right(), _loadPaletteButton.Bounds.Bottom() + 25);
        _colorsPanel.Bounds = Rectangles.FromLtrb(ContentBounds.Left(), _nextPaletteButton.Bounds.Bottom() + Padding, ContentBounds.Right(), ContentBounds.Bottom());

        _paletteNameLabel.Text = $"Selected:{(SelectedPalette == null ? "None" : SelectedPalette.PaletteName)}";

        _loadPaletteButton.Update(deltaTime);
        _paletteNameLabel.Update(deltaTime);
        
        _nextPaletteButton.Update(deltaTime);
        _nextPaletteButton.Enabled = _selectedColorPalette < _colorPalettes.Count - 1;
        
        _prevPaletteButton.Update(deltaTime);
        _prevPaletteButton.Enabled = _selectedColorPalette > 0;
        
        _colorsPanel.Update(deltaTime);
        _colorsPanel.ColorPalette = SelectedPalette;
    }

    private class PalettePanel : Panel
    {
        public ColorPalette ColorPalette { get; set; }

        private int _selectedColorIndex = 0;
        public Color SelectedColor => ColorPalette.GetColor(_selectedColorIndex);
        public int ColorPadding { get; set; } = 2;

        private const int ColorsPerLine = 6;
        private const int SpaceCount = ColorsPerLine - 1;
        private int _colorButtonSize = 0;

        public PalettePanel()
        {
            Padding = 0;
        }

        public override void Draw()
        {
            base.Draw();

            if (ColorPalette == null) return;

            var mousePosition = Raylib.GetMousePosition();
            var x = ContentBounds.x;
            var y = ContentBounds.y;

            for (var i = 0; i < ColorPalette.Colors.Count; i++)
            {
                if (i > 0 && i % ColorsPerLine == 0)
                {
                    x = ContentBounds.x;
                    y += _colorButtonSize + ColorPadding;
                }

                var bounds = new Rectangle(x, y, _colorButtonSize, _colorButtonSize);
                var hovered = Raylib.CheckCollisionPointRec(mousePosition, bounds);
                var selected = _selectedColorIndex == i;
                var color = ColorPalette.GetColor(i);

                Raylib.DrawRectangleRec(bounds, color);
                if (hovered && !selected)
                {
                    Raylib.DrawRectangleLinesEx(bounds, 1f, Colors.Black);
                }
                else if (selected)
                {
                    Raylib.DrawRectangleLinesEx(bounds, 2f, Colors.GetBestVisibleColor(color));
                }

                x += _colorButtonSize + ColorPadding;
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (ColorPalette == null) return;

            _colorButtonSize = (int) ((ContentBounds.width - SpaceCount * ColorPadding) / ColorsPerLine);

            var mousePosition = Raylib.GetMousePosition();
            var x = ContentBounds.x;
            var y = ContentBounds.y;

            for (var i = 0; i < ColorPalette.Colors.Count; i++)
            {
                if (i > 0 && i % ColorsPerLine == 0)
                {
                    x = ContentBounds.x;
                    y += _colorButtonSize + ColorPadding;
                }

                var bounds = new Rectangle(x, y, _colorButtonSize, _colorButtonSize);
                if (Raylib.CheckCollisionPointRec(mousePosition, bounds))
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    {
                        _selectedColorIndex = i;
                        Console.WriteLine(i);
                    }
                }

                x += _colorButtonSize + ColorPadding;
            }
        }
    }
}