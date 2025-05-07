using Microsoft.AspNetCore.Http;

namespace CleanKit.Net.Presentation.Extensions;

public static class HttpContextExtensions
{
    public static string GetConnectionIpAddress(this HttpContext httpContext)
    {
        if (httpContext.Request.Headers.ContainsKey("CF-Connecting-IP"))
            return httpContext.Request.Headers["CF-Connecting-IP"]!;
        if (httpContext.Request.Headers.ContainsKey("HTTP-X-FORWARDED-FOR"))
            return httpContext.Request.Headers["HTTP-X-FORWARDED-FOR"]!;
        return httpContext.Connection.RemoteIpAddress!.ToString();
    }
}