using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CleanKit.Net.Application.Abstractions.Providers;

public interface IJwtProvider
{
    string Generate(TimeSpan timeToLive, params Claim[] claims);
    JwtSecurityToken Validate(string token);
}