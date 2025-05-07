namespace CleanKit.Net.Utils;

public static class ArrayExtensions
{
    public static string ToHtmlQuery<T>(this T[] array, string name)
    {
        var query = string.Join('&', array.Select(value => $"{name}={value}"));
        if (!string.IsNullOrEmpty(query)) query = "&" + query;
        return query;
    }
}