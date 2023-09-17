using System.Numerics;

namespace Spritery.Utils;

public static class VectorExtensions
{
    public static Vector2 Offset(this Vector2 vector2, float x, float y) => new(vector2.X + x, vector2.Y + y);
}