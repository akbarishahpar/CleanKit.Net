namespace CleanKit.Net.Template.Presentation.WebApi.Models;

public class WeatherForecast
{
    public DateOnly Date { get; init; }
    public int TemperatureC { get; init; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}