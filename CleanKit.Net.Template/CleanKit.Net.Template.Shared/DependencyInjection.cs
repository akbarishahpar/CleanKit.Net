using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CleanKit.Net.Template.SharedSettings;

public static class DependencyInjection
{
    public static IConfigurationBuilder AddSharedSettings(this IConfigurationBuilder configuration)
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (string.IsNullOrEmpty(basePath))
            throw new Exception("Could not acquire path of executing assembly");
        return configuration
            .AddJsonFile(Path.Combine(basePath, "appsettings-shared.Development.json"), false, false)
            .AddJsonFile(Path.Combine(basePath, "appsettings-shared.json"), false, false);
    }

    public static void ConfigureSerilog(this ConfigureHostBuilder host, IConfiguration configuration)
    {
        host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
    }
}