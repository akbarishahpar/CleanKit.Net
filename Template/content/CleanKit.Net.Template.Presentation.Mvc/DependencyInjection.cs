using CleanKit.Net.Application.Abstractions.Providers;
using CleanKit.Net.DependencyInjection;
using CleanKit.Net.Presentation;
using CleanKit.Net.Presentation.Providers;

namespace CleanKit.Net.Template.Presentation.Mvc;

public static class DependencyInjection
{
    public static void AddPresentation(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>(configuration);
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddAuthentication().AddCookie(options =>
        {
            options.LoginPath = "/identity/login"; // TODO: MVC LOGIN URL GOES HERE
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
        });
        
        services.AddControllersWithViews()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);
    }
}