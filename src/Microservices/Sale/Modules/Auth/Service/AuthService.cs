using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using Novell.Directory.Ldap;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private ILDAPUtils _ldapUtils;
    private readonly IConfiguration _configuration;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils, ILDAPUtils ldapUtils)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
        _ldapUtils = ldapUtils;
    }

    public BaseResponse Login(LoginRequest login)
    {
        // validate account LDAP
       //  _ldapUtils.CheckUserLoginLdap(login.username, login.password);
        
        // Query Data SPLDB Get info and role to userInfo
        UserInfo userInfo = new UserInfo();
        userInfo.firstname = "test";
        userInfo.lastname = "test";
        userInfo.email = "email";
        userInfo.user_id = "supachai";
        userInfo.username = "username";
        userInfo.role_name = new string[]{"admin","user"};
        // build token
        LoginResponse response = new LoginResponse();
        response.token = _jwtUtils.GenerateJwtToken(userInfo.user_id);;
        response.refresh_token = _jwtUtils.GenerateRefreshToken(userInfo.user_id);;
        response.user_info = userInfo;
        
        
        //terminate old session in redis
        return new BaseResponse(new StatusResponse(), response);
    }

    public BaseResponse RefreshToken(string userId)
    {
        // Query Data SPLDB Get info and role to userInfo
        UserInfo userInfo = new UserInfo();
        userInfo.firstname = "test";
        userInfo.lastname = "test";
        userInfo.email = "email";
        userInfo.user_id = "id";
        userInfo.username = "username";
        userInfo.role_name = new string[]{"admin","user"};
        
        
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(userInfo.user_id),
            refresh_token = _jwtUtils.GenerateRefreshToken(userInfo.user_id)
        };
        return new BaseResponse(new StatusResponse(), response);
    }
}