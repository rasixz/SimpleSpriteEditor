using System.Collections.ObjectModel;
using Raylib_cs;

namespace Spritery.Editor;

public class ColorPalette
{
    private List<Color> _colors = new();
    public ReadOnlyCollection<Color> Colors => _colors.AsReadOnly();

    public string PaletteName { get; }

    public string Description { get; }

    public ColorPalette(string paletteName, string description)
    {
        PaletteName = paletteName;
        Description = description;
    }

    public void AddColor(Color color)
    {
        _colors.Add(color);
    }

    public Color GetColor(int index)
    {
        return _colors[index];
    }

    public static ColorPalette FromPaintNetTxt(string text)
    {
        var name = "";
        var description = "";
        var colors = new List<Color>();
        var splitter = text.Contains("\r\n") ? "\r\n" : text.Contains('\r') ? "\r" : "\n";

        var lines = text.Split(splitter);
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line)) continue;
            if (line.StartsWith(";"))
            {
                var key = line[1..];
                if (!key.Contains(':')) continue;

                var value = key[(key.IndexOf(':') + 1)..];
                key = key[..key.IndexOf(':')];

                switch (key)
                {
                    case "Palette Name":
                        name = value;
                        break;
                    case "Description":
                        description = value;
                        break;
                }

                continue;
            }

            var color = Utils.Colors.FromArgb(line);
            colors.Add(color);
        }

        return new ColorPalette(name, description)
        {
            _colors = colors
        };
    }

    public static ColorPalette FromPaintNetTxtFile(string path)
    {
        return FromPaintNetTxt(File.ReadAllText(path));
    }
}