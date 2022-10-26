using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using Microsoft.AspNetCore.Http.Headers;
namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private readonly IConfiguration _configuration;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
    }
    public BaseResponse Login(LoginRequest login)
    {
        // validate account LDAP
        // Query Data SPLDB Get info and role
        // build token
       // string token = BuildToken();
        
       var token=_jwtUtils.GenerateJwtToken("supacjai");
       //terminate session
        //
        // LoginResponse response = new LoginResponse();
        // response.token = token;
        // response.refresh_token = token;
        //
        // UserInfo userInfo = new UserInfo();
        // userInfo.firstname = "test";
        // userInfo.lastname = "test";
        // userInfo.email = "email";
        // userInfo.user_id = "id";
        // userInfo.username = "username";
        // userInfo.role_name = new string[]{"admin","user"};
        // response.user_info = userInfo;
        
        return new BaseResponse(new StatusResponse(), token);
    }

    public BaseResponse RefreshToken(string userId)
    {
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(userId),
            refresh_token = _jwtUtils.GenerateRefreshToken(userId)
        };
        return new BaseResponse(new StatusResponse(), response);
    }
}