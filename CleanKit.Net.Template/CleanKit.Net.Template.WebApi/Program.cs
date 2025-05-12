using CleanKit.Net.Presentation;
using CleanKit.Net.Presentation.Middleware;
using CleanKit.Net.Template.SharedSettings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Configure appsettings
builder.Configuration.AddSharedSettings();
builder.Host.ConfigureSerilog(builder.Configuration);

// Add services to the container.
CleanKit.Net.Template.Application.DependencyInjection.AddApplication(builder.Services, builder.Configuration);
CleanKit.Net.Template.Infrastructure.DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);
CleanKit.Net.Template.Persistence.DependencyInjection.AddPersistence(builder.Services, builder.Configuration);
CleanKit.Net.Template.Presentation.WebApi.DependencyInjection.AddPresentation(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseReDoc();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseExceptionCapturing();

app.UseAuthorization();

app.UseNormalizer();

app.MapControllers();

app.Run();
