using System.Web;
using CleanKit.Net.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CleanKit.Net.Presentation.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNormalizer(this IApplicationBuilder app) => app.Use(
            async (context, next) =>
            {
                context.Request.QueryString = new QueryString(
                    StringExtensions
                        .Normalize(HttpUtility.UrlDecode(context.Request.QueryString.Value))?
                        .Fa2En()
                );

                if (context.Request.HasFormContentType)
                {
                    var fields = context.Request.Form.Keys.ToDictionary<string, string, StringValues>(key => key,
                        key => context.Request.Form[key].Select(Utils.StringExtensions.Normalize).ToArray());
                    context.Request.Form = new FormCollection(fields, context.Request.Form.Files);
                }

                await next();
            });
    }
}