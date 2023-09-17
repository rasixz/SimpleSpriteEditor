namespace Spritery.Utils;

public static class StringExtensions
{
    public static string SubstringPos(this string str, int start, int end)
    {
        return str.Substring(start, end - start);
    }
}