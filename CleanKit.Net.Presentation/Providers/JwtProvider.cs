using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanKit.Net.Application.Abstractions.Providers;
using Microsoft.IdentityModel.Tokens;

namespace CleanKit.Net.Presentation.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(JwtOptions options)
    {
        _options = options;
    }

    public string Generate(TimeSpan timeToLive, params Claim[] claims)
    {
        var signatureKey = Encoding.UTF8.GetBytes(_options.SignatureKey); // longer than 16 character
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(signatureKey),
            SecurityAlgorithms.HmacSha256Signature);

        var encryptionKey = Encoding.UTF8.GetBytes(_options.EncryptionKey); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey),
            SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(timeToLive),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public JwtSecurityToken Validate(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var signatureKey = Encoding.UTF8.GetBytes(_options.SignatureKey); // longer that 16 character
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(signatureKey),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingCredentials.Key,
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ClockSkew = TimeSpan.Zero,
        };

        if (!string.IsNullOrEmpty(_options.EncryptionKey))
        {
            var encryptionKey = Encoding.UTF8.GetBytes(_options.EncryptionKey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey),
                SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            tokenValidationParameters.TokenDecryptionKey = encryptingCredentials.Key;
        }

        tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out var validatedToken
        );

        return (JwtSecurityToken)validatedToken;
    }
}