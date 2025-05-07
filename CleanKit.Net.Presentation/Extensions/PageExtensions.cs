namespace CleanKit.Net.Presentation.Extensions;

public static class PageExtensions
{
    public static string Simplify(this string url)
    {
        return url
            .Replace("Pages/", "")
            .Replace(".cshtml", "");
    }
}