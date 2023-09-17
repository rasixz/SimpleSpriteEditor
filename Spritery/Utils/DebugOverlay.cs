using Raylib_cs;

namespace Spritery.Utils;

public static class DebugOverlay
{
    private const int FontHeight = 15;
    private static int _drawYLeft = 5;
    private static int _drawYRight = 5;

    private static Rectangle _bounds;
    
    public static void Begin(Rectangle bounds)
    {
        _bounds = bounds;
        _drawYLeft = 5;
        _drawYRight = 5;
    }
    
    public static void WriteLeft(string text)
    {
        Raylib.DrawText(text, (int)_bounds.Left() + 5, _drawYLeft, FontHeight, Colors.Black);
        _drawYLeft += 5 + FontHeight;
    }
    
    public static void WriteRight(string text)
    {
        Raylib.DrawText(text,  (int)_bounds.Right() - Raylib.MeasureText(text, FontHeight) - 5, _drawYRight, FontHeight, Colors.Black);
        _drawYRight += 5 + FontHeight;
    }
}