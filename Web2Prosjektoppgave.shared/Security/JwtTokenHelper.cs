using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Web2Prosjektoppgave.shared.Security;

public static class JwtTokenHelper
{
    public static string CreateJWT(List<Claim> claims)

    {
        var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials
        );

        var securityTokenHandler = new JwtSecurityTokenHandler();

        return securityTokenHandler.WriteToken(securityToken);
    }

    public static List<Claim> DecodeJwt(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidIssuer = Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return new List<Claim>();
        }

        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return securityToken.Claims.ToList();
    }

    public static string Issuer
    {
        get
        {
            return "https://localhost:7238/";
        }
    }

    public static string Audience
    {
        get
        {
            return "https://localhost:7238/";
        }
    }

    public static string Key
    {
        get
        {
            return "SECRET-KEY-MUST-BE-AT-LEAST-128-BITS-LONG";
        }
    }
}