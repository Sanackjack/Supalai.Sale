using Spl.Crm.SaleOrder.Modules.Auth.Model;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    
    public AuthService(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    public BaseResponse Login(LoginRequest login)
    {
        // validate account LDAP
        // Query Data SPLDB Get info and role
        // build token
        string token = BuildToken();
        //terminate session

        LoginResponse response = new LoginResponse();
        response.token = token;
        response.refresh_token = token;

        UserInfo userInfo = new UserInfo();
        userInfo.firstname = "test";
        userInfo.lastname = "test";
        userInfo.email = "email";
        userInfo.user_id = "id";
        userInfo.username = "username";
        userInfo.role_name = new string[]{"admin","user"};
        response.user_info = userInfo;
        
        return new BaseResponse(new StatusResponse(), response);
    }

    private string BuildToken()
    {
        var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtSettings:Expire"]));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, "test"),
            new Claim("role", "Admin"),
            new Claim("additional", "todo"),
        };
        
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}