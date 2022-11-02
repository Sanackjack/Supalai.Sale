#nullable enable
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using Microsoft.Extensions.Configuration;

namespace ClassifiedAds.Infrastructure.JWT;
public interface IJwtUtils
{
    public string GenerateJwtToken(TokenInfo tokenInfo);
    public TokenInfo? ValidateJwtToken(string token);
    public string GenerateRefreshToken(TokenInfo tokenInfo);
}

public class JwtUtils : IJwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string GenerateJwtToken(TokenInfo tokenInfo)
    {
        List<Claim> claims = BuildCliams(tokenInfo, false);
        return BuildToken(Convert.ToDouble(_configuration["JwtSettings:Expire"]), claims);
    }

    public string GenerateRefreshToken(TokenInfo tokenInfo)
    {
        List<Claim> claims = BuildCliams(tokenInfo, true);
        return BuildToken(Convert.ToDouble(_configuration["JwtSettings:Expire"]) + 4.00f, claims);
    }

    private List<Claim> BuildCliams(TokenInfo tokenInfo, bool isRefreshToken)
    {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(nameof(TokenInfo.is_refresh_token), isRefreshToken ? "true" : "false"));
        claims.Add(new Claim(nameof(TokenInfo.user_id), tokenInfo.user_id));
        claims.Add(new Claim(nameof(TokenInfo.username), tokenInfo.username));
        claims.Add(new Claim(nameof(TokenInfo.firstname), tokenInfo.firstname));
        claims.Add(new Claim(nameof(TokenInfo.lastname), tokenInfo.lastname));
        claims.Add(new Claim(nameof(TokenInfo.email), tokenInfo.email));
        foreach (var role in tokenInfo.role)
        {
            claims.Add(new Claim(nameof(TokenInfo.role), role));
        }
        return claims;
    }

    private string BuildToken(double expire, IEnumerable<Claim> claim)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claim),
            Expires = DateTime.UtcNow.AddHours(expire),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"])), SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public TokenInfo? ValidateJwtToken(string token)
    {
        if (string.IsNullOrEmpty(token))
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
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            tokenInfo.user_id = (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.user_id)).Value;
            tokenInfo.firstname = (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.firstname)).Value;
            tokenInfo.lastname = (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.lastname)).Value;
            tokenInfo.username = (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.username)).Value;
            tokenInfo.email = (string)jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.email)).Value;
            tokenInfo.payload = token.Split(".")[1];
            tokenInfo.role = jwtToken.Claims.Where(x => x.Type == nameof(TokenInfo.role)).Select(y => y.Value).ToList();
            tokenInfo.is_refresh_token = bool.Parse(jwtToken.Claims.First(x => x.Type == nameof(TokenInfo.is_refresh_token)).Value.ToString());
            return tokenInfo;
        }
        catch (SecurityTokenExpiredException e)
        {
            throw new AuthenicationErrorException(ResponseData.TOKEN_EXPIRED);
        }
        catch (Exception e)
        {
            throw new AuthenicationErrorException(ResponseData.TOKEN_INVALID);
        }
    }
}
