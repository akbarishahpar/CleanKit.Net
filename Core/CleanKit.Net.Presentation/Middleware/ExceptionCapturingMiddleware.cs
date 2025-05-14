using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using CleanKit.Net.Presentation.Options;

namespace CleanKit.Net.Presentation.Middleware
{
    public class ExceptionCapturingMiddleware : IMiddleware
    {
        private readonly ExceptionCapturingOptions _options;

        public ExceptionCapturingMiddleware(ExceptionCapturingOptions options)
        {
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //Setting up the stopwatch
            var sp = new Stopwatch();
            sp.Start();

            try
            {
                await next(context); //Executing the next middleware
            }
            catch (Exception exception)
            {
                LogError(context, exception, sp.ElapsedMilliseconds);
                if (!_options.Passthrough)
                    await WriteToResponseAsync(context, exception);
            }
        }

        private static void LogError(
            HttpContext context,
            Exception exception,
            long elapsedMilliseconds
        )
        {
            var logger = context.RequestServices.GetRequiredService<
                ILogger<ExceptionCapturingMiddleware>
            >();
            logger.LogError(
                "{@Type} occurred at {@DateTimeUtc} in {@ElapsedMilliseconds}ms: {@Message}\n{@StackTrace}",
                exception.GetType().Name,
                DateTime.UtcNow,
                elapsedMilliseconds,
                exception.Message,
                exception.StackTrace
            );
        }

        private static async Task WriteToResponseAsync(HttpContext context, Exception exception)
        {
            var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/plain";
            if (env.IsDevelopment())
                await context.Response.WriteAsync(
                    $"{exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}",
                    context.RequestAborted
                );
            else
                await context.Response.WriteAsync(string.Empty, context.RequestAborted);
        }
    }
}