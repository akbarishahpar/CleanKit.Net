using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddHttpClientWithGzipSupport(this IServiceCollection services)
    {
        services.AddHttpClient("")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
    }
}