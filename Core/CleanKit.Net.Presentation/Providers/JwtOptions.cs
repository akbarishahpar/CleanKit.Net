namespace CleanKit.Net.Presentation.Providers;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SignatureKey { get; set; } = string.Empty;
    public string EncryptionKey { get; set; } = string.Empty;
}