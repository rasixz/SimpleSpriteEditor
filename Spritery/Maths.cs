namespace Spritery;

public static class Maths
{
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // Calculate the range of the input value
        var fromRange = fromMax - fromMin;

        // Calculate the range of the output value
        var toRange = toMax - toMin;

        // Scale the input value to the output range
        var scaledValue = (value - fromMin) * toRange / fromRange;

        // Map the scaled value to the output range
        var mappedValue = scaledValue + toMin;

        return mappedValue;
    }
}