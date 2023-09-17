using System.Collections.ObjectModel;
using System.Drawing;
using SkiaSharp;
using Spritery.Utils;
using Color = Raylib_cs.Color;

namespace Spritery.Editor;

public sealed class Sprite
{
    public int Width { get; }
    public int Height { get; }

    private readonly List<Color> _colors;

    public ReadOnlyCollection<Color> Colors => _colors.AsReadOnly();

    public Sprite(int size) : this(size, size)
    {
    }

    public Sprite(int width, int height)
    {
        width = (int) MathF.Min(width, 1024);
        height = (int) MathF.Min(height, 1024);

        Width = width;
        Height = height;

        _colors = Enumerable.Repeat(Utils.Colors.Invisible, Width * Height).ToList();
    }

    public Color GetPixel(int x, int y)
    {
        return _colors[y * Width + x];
    }

    public Color GetPixel(Point point)
    {
        return GetPixel(point.X, point.Y);
    }

    public void SetPixel(int x, int y, Color color)
    {
        _colors[y * Width + x] = color;
    }

    public void SetPixel(Point p, Color color)
    {
        SetPixel(p.X, p.Y, color);
    }

    private List<int> GetArgb32Pixels() => _colors.Select(color => color.ToArgb()).ToList();

    /// <summary>
    /// Exports a sprite to a given path as PNG
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="path"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Export(Sprite sprite, string path)
    {
        var imgWidth = sprite.Width;
        var imgHeight = sprite.Height;

        var bitmap = new SKBitmap(imgWidth, imgHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
        var colors = sprite._colors;

        if (bitmap == null)
            throw new ArgumentNullException(nameof(sprite));

        if (colors.Count != bitmap.Width * bitmap.Height)
            throw new ArgumentException("The pixel array length must match the bitmap dimensions.");

        for (var i = 0; i < colors.Count; i++)
        {
            var color = colors[i];

            bitmap.SetPixel(i % bitmap.Width, i / bitmap.Width, new SKColor(color.r, color.g, color.b, color.a));
        }

        using var stream = new SKFileWStream(path);
        bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
    }

    // TODO:
    public static Sprite Import(string path)
    {
        using var stream = new SKFileStream(path);
        var bitmap = SKBitmap.Decode(stream);

        var sprite = new Sprite(bitmap.Width, bitmap.Height);

        using var pixmap = bitmap.PeekPixels();
        var pixelSpan = pixmap.GetPixelSpan();

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var r = pixelSpan[(x * 4) + (y * pixmap.RowBytes) + 0];
                var g = pixelSpan[(x * 4) + (y * pixmap.RowBytes) + 1];
                var b = pixelSpan[(x * 4) + (y * pixmap.RowBytes) + 2];
                var a = pixelSpan[(x * 4) + (y * pixmap.RowBytes) + 3];

                sprite.SetPixel(x, y, new Color(r, g, b, a));
            }
        }

        return sprite;
    }

    public bool IsEmpty()
    {
        return _colors.All(c => c.Equals(Utils.Colors.Invisible));
    }
}