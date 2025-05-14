using System.Text.Json;

namespace CleanKit.Net.Utils;

public static class JsonElementExtensions
{
    public static JsonElement? TryGetProperty(this JsonElement jsonElement, string propertyName)
    {
        return jsonElement.TryGetProperty(propertyName, out var result) ? result : null;
    }
}