using CleanKit.Net.Application.Abstractions.Providers;
using CleanKit.Net.DependencyInjection;
using CleanKit.Net.Presentation;
using CleanKit.Net.Presentation.Providers;

namespace CleanKit.Net.Template.Presentation.RazorPages;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>(configuration);
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddAuthentication().AddCookie(options =>
        {
            options.LoginPath = "/identity/login"; // TODO: RAZOR PAGES LOGIN URL GOES HERE
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
        });
        
        services.AddRazorPages()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);
    }
}