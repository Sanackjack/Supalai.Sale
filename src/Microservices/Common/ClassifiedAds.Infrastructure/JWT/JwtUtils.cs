#nullable enable
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ClassifiedAds.Infrastructure.JWT;
public interface IJwtUtils
{
    public string GenerateJwtToken(string userId);
    public TokenInfo? ValidateJwtToken(string token);
    public string GenerateRefreshToken(string userId);
}

public class JwtUtils : IJwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string GenerateJwtToken(string userId)
    {
        List<Claim> claims = BuildCliams(userId, false);
        return BuildToken(Convert.ToDouble(_configuration["JwtSettings:Expire"]), claims);
    }

    public string GenerateRefreshToken(string userId)
    {
        List<Claim> claims = BuildCliams(userId, true);
        return BuildToken(Convert.ToDouble(_configuration["JwtSettings:Expire"]) + 4.00f, claims);
    }

    private List<Claim> BuildCliams(string userId, bool isRefreshToken)
    {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(nameof(TokenInfo.user_id), userId));
        claims.Add(new Claim(nameof(TokenInfo.is_refresh_token), isRefreshToken ? "true" : "false"));
        return claims;
    }

    private string BuildToken(double expire, IEnumerable<Claim> claim)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claim),
            Expires = DateTime.UtcNow.AddHours(expire),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"])), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public TokenInfo? ValidateJwtToken(string token)
    {
        if (token == null)
        {
            return null;
        }

        TokenInfo tokenInfo = new TokenInfo();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            tokenInfo.user_id = (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.user_id)).Value;
            tokenInfo.is_refresh_token =
                (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.is_refresh_token)).Value;
            tokenInfo.TokenStatus = TokenStatus.Success;
            return tokenInfo;
        }
        catch (SecurityTokenExpiredException e)
        {
            tokenInfo.TokenStatus = TokenStatus.Expire;
            return tokenInfo;
        }
        catch (Exception e)
        {
            tokenInfo.TokenStatus = TokenStatus.Error;
            return tokenInfo;
        }
    }
}
