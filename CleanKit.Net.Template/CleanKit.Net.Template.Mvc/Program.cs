using CleanKit.Net.Presentation.Middleware;
using CleanKit.Net.Template.SharedSettings;

var builder = WebApplication.CreateBuilder(args);

//Configure appsettings
builder.Configuration.AddSharedSettings();
builder.Host.ConfigureSerilog(builder.Configuration);

// Add services to the container.
CleanKit.Net.Template.Application.DependencyInjection.AddApplication(builder.Services, builder.Configuration);
CleanKit.Net.Template.Infrastructure.DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);
CleanKit.Net.Template.Persistence.DependencyInjection.AddPersistence(builder.Services, builder.Configuration);
CleanKit.Net.Template.Presentation.Mvc.DependencyInjection.AddPresentation(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStatusCodePagesWithReExecute("/Errors/Error{0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error500");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseNormalizer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
