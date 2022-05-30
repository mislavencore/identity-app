using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.WebToken
{
    public interface IWebTokenService
    {
        string GenerateRefreshToken();
        JwtSecurityToken CreateToken(string username, bool isAdmin = false);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}