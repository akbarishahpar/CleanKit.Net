using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using CleanKit.Net.Presentation;
using Microsoft.OpenApi.Models;

namespace CleanKit.Net.Template.Presentation.WebApi;

public static class DependencyInjection
{
    public static void AddPresentation(IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionCapturing();
        services.AddJwtBearer(configuration);

        services.AddSwagger(
            new OpenApiInfo
            {
                Title = "WebApi Documentation",
                Version = "v1",
                Description = "The description of your api goes here",
                Contact = new OpenApiContact { Name = "Dev team", Email = "dev@cleankit.net" }
            },
            typeof(DependencyInjection).Assembly
        );

        services
            .AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
    }
}