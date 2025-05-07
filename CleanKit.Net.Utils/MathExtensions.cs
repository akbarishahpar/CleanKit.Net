namespace CleanKit.Net.Utils;

public static class MathExtensions
{
    public static decimal Truncate(this decimal decimalValue, int decimalPlaces)
    {
        var factor = (decimal)Math.Pow(10, decimalPlaces);
        var x = Math.Truncate(decimalValue * factor);
        return  x / factor;
    }
}