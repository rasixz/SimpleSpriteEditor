namespace Spritery;

public static class Interpolation
{
    public interface IEasing
    {
        float Calculate(float t);
    }

    public static float Lerp(float a, float b, float t, IEasing easing) => a + (b - a) * easing.Calculate(t);

    public class Linear : IEasing
    {
        public float Calculate(float t)
        {
            return t;
        }
    }

    public class Ease : IEasing
    {
        public float Calculate(float t)
        {
            return t * t * (3 - 2 * t);
        }
    }

    public class EaseIn : IEasing
    {
        public float Calculate(float t)
        {
            return t * t;
        }
    }

    public class EaseOut : IEasing
    {
        public float Calculate(float t)
        {
            return t * (2 - t);
        }
    }

    public class EaseInOut : IEasing
    {
        public float Calculate(float t)
        {
            return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }
    }

    public class CubicBezier : IEasing
    {
        private readonly float[] _controlPoints;

        public CubicBezier(params float[] controlPoints)
        {
            _controlPoints = controlPoints;
        }


        public float Calculate(float t)
        {
            var result = 0.0f;
            var n = _controlPoints.Length - 1;

            for (var i = 0; i <= n; i++)
            {
                result += _controlPoints[i] * BinomialCoefficient(n, i) * MathF.Pow(t, i) * MathF.Pow(1 - t, n - i);
            }

            return result;
        }

        private static long BinomialCoefficient(int n, int k)
        {
            if (k < 0 || k > n)
            {
                return 0;
            }

            long coeff = 1;

            for (var i = 0; i < k; i++)
            {
                coeff *= (n - i);
                coeff /= (i + 1);
            }

            return coeff;
        }
    }

    public class StepStart : IEasing
    {
        public float Calculate(float t) => t < 1.0f ? 0.0f : 1.0f;
    }

    public class StepEnd : IEasing
    {
        public float Calculate(float t) => t < 1.0f ? 0.0f : 1.0f;
    }

    public class Steps : IEasing
    {
        private readonly int _steps;

        public Steps(int steps)
        {
            if (steps <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steps));
            }

            _steps = steps;
        }


        public float Calculate(float t)
        {
            return MathF.Floor(t * _steps) / _steps;
        }
    }

    public class BounceIn : IEasing
    {
        public float Calculate(float t)
        {
            return 1.0f - PerformBounceOut(1.0f - t);
        }

        private static float PerformBounceOut(float t)
        {
            switch (t)
            {
                case < 1f / 2.75f:
                    return 7.5625f * t * t;
                case < 2f / 2.75f:
                    t -= 1.5f / 2.75f;
                    return 7.5625f * t * t + 0.75f;
                case < 2.5f / 2.75f:
                    t -= 2.25f / 2.75f;
                    return 7.5625f * t * t + 0.9375f;
                default:
                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
            }
        }
    }

    public class BounceOut : IEasing
    {
        public float Calculate(float t)
        {
            switch (t)
            {
                case < 1f / 2.75f:
                    return 7.5625f * t * t;
                case < 2f / 2.75f:
                    t -= 1.5f / 2.75f;
                    return 7.5625f * t * t + 0.75f;
                case < 2.5f / 2.75f:
                    t -= 2.25f / 2.75f;
                    return 7.5625f * t * t + 0.9375f;
                default:
                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
            }
        }
    }

    public class BounceInOut : IEasing
    {
        public float Calculate(float t)
        {
            if (t < 0.5f)
            {
                return 0.5f * PerformBounceIn(t * 2f);
            }

            return 0.5f * PerformBounceOut(t * 2f - 1f) + 0.5f;
        }

        private static float PerformBounceIn(float t)
        {
            return 1.0f - PerformBounceOut(1.0f - t);
        }

        private static float PerformBounceOut(float t)
        {
            switch (t)
            {
                case < 1f / 2.75f:
                    return 7.5625f * t * t;
                case < 2f / 2.75f:
                    t -= 1.5f / 2.75f;
                    return 7.5625f * t * t + 0.75f;
                case < 2.5f / 2.75f:
                    t -= 2.25f / 2.75f;
                    return 7.5625f * t * t + 0.9375f;
                default:
                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
            }
        }
    }

    public class SineIn : IEasing
    {
        public float Calculate(float t)
        {
            return 1.0f - MathF.Cos(t * MathF.PI / 2.0f);
        }
    }

    public class SineOut : IEasing
    {
        public float Calculate(float t)
        {
            return MathF.Sin(t * MathF.PI / 2.0f);
        }
    }

    public class SineInOut : IEasing
    {
        public float Calculate(float t)
        {
            return -0.5f * (MathF.Cos(MathF.PI * t) - 1.0f);
        }
    }

    public class ElasticIn : IEasing
    {
        private readonly float _amplitude;
        private readonly float _period;

        public ElasticIn(float amplitude, float period)
        {
            _amplitude = amplitude;
            _period = period;
        }


        public float Calculate(float t)
        {
            if ((int) t == 0 || (int) t == 1)
            {
                return t;
            }

            var s = _period / (2.0f * Math.PI) * MathF.Asin(1.0f / _amplitude);
            return -(_amplitude * MathF.Pow(2f, 10.0f * (t -= 1.0f)) * MathF.Sin((float) ((t - s) * (2.0f * MathF.PI) / _period)));
        }
    }

    public class ElasticOut : IEasing
    {
        private readonly float _amplitude;
        private readonly float _period;

        public ElasticOut(float amplitude, float period)
        {
            _amplitude = amplitude;
            _period = period;
        }


        public float Calculate(float t)
        {
            if ((int) t == 0 || (int) t == 1)
            {
                return t;
            }
            else
            {
                var s = _period / (2.0f * MathF.PI) * MathF.Asin(1.0f / _amplitude);
                return _amplitude * MathF.Pow(2.0f, -10.0f * t) * MathF.Sin((t - s) * (2.0f * MathF.PI) / _period) + 1.0f;
            }
        }
    }

    public class ElasticInOut : IEasing
    {
        private readonly float _amplitude;
        private readonly float _period;

        public ElasticInOut(float amplitude, float period)
        {
            _amplitude = amplitude;
            _period = period;
        }


        public float Calculate(float t)
        {
            if ((int) t == 0 || (int) t == 1)
            {
                return t;
            }

            var s = _period / (2.0f * MathF.PI) * MathF.Asin(1.0f / _amplitude);
            if ((t *= 2.0f) < 1.0f)
            {
                return -0.5f * (_amplitude * MathF.Pow(2.0f, 10.0f * (t -= 1.0f)) * MathF.Sin((t - s) * (2.0f * MathF.PI) / _period));
            }

            return _amplitude * MathF.Pow(2.0f, -10.0f * (t -= 1.0f)) * MathF.Sin((t - s) * (2.0f * MathF.PI) / _period) * 0.5f + 1.0f;
        }
    }

    public class BackIn : IEasing
    {
        private readonly float _overshoot;

        public BackIn(float overshoot)
        {
            _overshoot = overshoot;
        }


        public float Calculate(float t)
        {
            return t * t * ((_overshoot + 1.0f) * t - _overshoot);
        }
    }

    public class BackOut : IEasing
    {
        private readonly float _overshoot;

        public BackOut(float overshoot)
        {
            _overshoot = overshoot;
        }


        public float Calculate(float t)
        {
            return 1.0f - ((t -= 1.0f) * t * ((_overshoot + 1.0f) * t + _overshoot));
        }
    }

    public class BackInOut : IEasing
    {
        private readonly float _overshoot;

        public BackInOut(float overshoot)
        {
            this._overshoot = overshoot;
        }


        public float Calculate(float t)
        {
            if ((t *= 2.0f) < 1.0f)
            {
                return 0.5f * (t * t * ((_overshoot * 1.525f + 1.0f) * t - _overshoot * 1.525f));
            }

            return 0.5f * ((t -= 2.0f) * t * ((_overshoot * 1.525f + 1.0f) * t + _overshoot * 1.525f) + 2.0f);
        }
    }

    public class ExponentialIn : IEasing
    {
        public float Calculate(float t)
        {
            return t == 0.0f ? 0.0f : MathF.Pow(2.0f, 10.0f * (t - 1.0f));
        }
    }

    public class ExponentialOut : IEasing
    {
        public float Calculate(float t)
        {
            return (int)t == 1 ? 1.0f : 1.0f - MathF.Pow(2.0f, -10.0f * t);
        }
    }

    public class ExponentialInOut : IEasing
    {
        public float Calculate(float t)
        {
            switch ((int) t)
            {
                case 0:
                    return 0.0f;
                case 1:
                    return 1.0f;
                default:
                {
                    if ((t *= 2.0f) < 1.0f)
                    {
                        return 0.5f * MathF.Pow(2.0f, 10.0f * (t - 1.0f));
                    }

                    return 0.5f * (2.0f - MathF.Pow(2.0f, -10.0f * (t - 1.0f)));
                }
            }
        }
    }

    public class CircularIn : IEasing
    {
        public float Calculate(float t)
        {
            return 1.0f - MathF.Sqrt(1.0f - t * t);
        }
    }

    public class CircularOut : IEasing
    {
        public float Calculate(float t)
        {
            return MathF.Sqrt(1.0f - (t -= 1.0f) * t);
        }
    }

    public class CircularInOut : IEasing
    {
        public float Calculate(float t)
        {
            if ((t *= 2.0f) < 1.0f)
            {
                return -0.5f * (MathF.Sqrt(1.0f - t * t) - 1.0f);
            }

            return 0.5f * (MathF.Sqrt(1.0f - (t -= 2.0f) * t) + 1.0f);
        }
    }
}