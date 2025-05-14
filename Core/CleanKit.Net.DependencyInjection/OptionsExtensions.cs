using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKit.Net.DependencyInjection;

public static class OptionsExtensions
{
    public static T AddOptions<T>(this IServiceCollection services, IConfiguration configuration) where T : class
    {
        var key = typeof(T).Name;
        var options = configuration
            .GetSection(key)
            .Get<T>();
        if (options is null)
            throw new KeyNotFoundException(
                $"Could not retrieve any configuration section for given key: '{key}'");
        services.AddSingleton<T>(_ => options);
        return options;
    }
}