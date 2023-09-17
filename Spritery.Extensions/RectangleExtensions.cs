using System.Numerics;
using Raylib_cs;

namespace Spritery.Utils;

public static class RectangleExtensions
{
    public static Vector2 TopLeft(this Rectangle rectangle) => new(rectangle.x, rectangle.y);
    public static Vector2 TopRight(this Rectangle rectangle) => new(rectangle.x + rectangle.width, rectangle.y);
    public static Vector2 BottomLeft(this Rectangle rectangle) => new(rectangle.x, rectangle.y + rectangle.height);
    public static Vector2 BottomRight(this Rectangle rectangle) => new(rectangle.x + rectangle.width, rectangle.y + rectangle.height);

    public static float Left(this Rectangle rectangle) => rectangle.x;
    public static float Top(this Rectangle rectangle) => rectangle.y;
    public static float Right(this Rectangle rectangle) => rectangle.x + rectangle.width;
    public static float Bottom(this Rectangle rectangle) => rectangle.y + rectangle.height;

    public static void SetX(this Rectangle rectangle, float x) => rectangle.x = x;
    public static void SetY(this Rectangle rectangle, float y) => rectangle.y = y;
    public static void SetWidth(this Rectangle rectangle, float width) => rectangle.width = width;
    public static void SetHeight(this Rectangle rectangle, float height) => rectangle.height = height;

    public static Rectangle Inset(this Rectangle rectangle, float inset) => new(rectangle.x + inset, rectangle.y + inset, rectangle.width - 2 * inset, rectangle.height - 2 * inset);

    public static void Set(this Rectangle rectangle, float x, float y, float width, float height)
    {
        rectangle.x = x;
        rectangle.y = y;
        rectangle.width = width;
        rectangle.height = height;
    }
}

public static class Rectangles
{
    public static Rectangle FromLtrb(float left, float top, float right, float bottom)
    {
        return new Rectangle(left, top, right - left, bottom - top);
    }
}