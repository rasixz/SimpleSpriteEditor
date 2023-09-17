using System.Diagnostics;

namespace Spritery.Utils;

public static class Diagnose
{
    public static (TimeSpan span, TResult result) Measure<TResult>(Func<TResult> function)
    {
        var watch = Stopwatch.StartNew();
        var result = function();
        return (watch.Elapsed, result);
    }

    public static TimeSpan Measure(Action function)
    {
        var watch = Stopwatch.StartNew();
        function();
        return watch.Elapsed;
    }
}