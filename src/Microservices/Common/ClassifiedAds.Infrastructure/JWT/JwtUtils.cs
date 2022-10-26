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
    public string? ValidateJwtToken(string token);
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
        return BuildToken(Convert.ToDouble(_configuration["JwtSettings:Expire"]),
            new[] { new Claim("user_id", userId) });
    }

    public string GenerateRefreshToken(string userId)
    {
        return BuildToken(Convert.ToDouble(_configuration["JwtSettings:Expire"]) + 4.00f,
            new[] { new Claim("user_id", userId, "isRefreshToken", "true") });
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

    public string? ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

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
            var userId = (string)jwtToken.Claims.First(x => x.Type == "user_id").Value;

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
}
