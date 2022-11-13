using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;
using ClassifiedAds.Domain.Uow;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;
using Twilio.Jwt.AccessToken;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private ILDAPUtils _ldapUtils;
    private readonly IConfiguration _configuration;
    private readonly ISysAdminUserRepository _sysAdminUserRepository;
    private readonly ISysAdminRoleRepository _sysAdminRoleRepository;
    private readonly ISaleOrderRepository _saleOrderRepository;
    private readonly IUnitOfWork _uow;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils, ILDAPUtils ldapUtils,ISysAdminUserRepository sysAdminUserRepository, IUnitOfWork uow, ISysAdminRoleRepository sysAdminRoleRepository, ISaleOrderRepository saleOrderRepository)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
        _ldapUtils = ldapUtils;
        _sysAdminUserRepository = sysAdminUserRepository;
        _sysAdminRoleRepository = sysAdminRoleRepository;
        _saleOrderRepository = saleOrderRepository;
        _uow = uow;
    }

    public BaseResponse Login(LoginRequest login)
    {
        //validate account LDAP
        _ldapUtils.CheckUserLoginLdap(login.username, login.password);
        
       //check user should has in DB
       SysUserInfo? sysUserinfo = _saleOrderRepository.FindSysUserInfoRawSqlByUserName(login.username);
       if (sysUserinfo == null)
            throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
       
       UserInfo userInfo = BuildUserInfo(sysUserinfo);
       
        // build token jwt
        TokenInfo tokenInfo = BuildTokenInfo(userInfo);
        LoginResponse response = new LoginResponse();
        response.token = _jwtUtils.GenerateJwtToken(tokenInfo);;
        response.refresh_token = _jwtUtils.GenerateRefreshToken(tokenInfo);;
        response.user_info = userInfo;
        
        //terminate old session in redis
        
        return new BaseResponse(new StatusResponse(), response);
    }

    public BaseResponse RefreshToken(TokenInfo token)
    {
        //check user should has in DB
        SysUserInfo? sysUserinfo = _saleOrderRepository.FindSysUserInfoRawSqlByUserName(token.username);
        if (sysUserinfo == null)
            throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
        UserInfo userInfo = BuildUserInfo(sysUserinfo);
        
        // build token jwt
        TokenInfo tokenInfo = BuildTokenInfo(userInfo);
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(tokenInfo),
            refresh_token = _jwtUtils.GenerateRefreshToken(tokenInfo)
        };
        
        //terminate old session in redis
        return new BaseResponse(new StatusResponse(), response);
    }

    private TokenInfo BuildTokenInfo(UserInfo userInfo)
    {
       return  new TokenInfo()
                    {
                        user_id = userInfo.user_id,
                        firstname = userInfo.firstname,
                        lastname = userInfo.lastname,
                        email = userInfo.email,
                        username = userInfo.username,
                        role = userInfo.role_name
                    };
    }
    
    private UserInfo BuildUserInfo(SysUserInfo sysUserinfo)
    {
        return   new UserInfo()
        {
            firstname = sysUserinfo.FirstName,
            lastname = sysUserinfo.LastName,
            email = sysUserinfo.Email,
            user_id = sysUserinfo.UserId,
            username = sysUserinfo.Username,
            role_name = new string[] { sysUserinfo.RoleName }
        };
    }

    public BaseResponse getUser()
    {
        SysUserInfo? sysUserinfo = _saleOrderRepository.FindSysUserInfoRawSqlByUserName("pimpaka.pie");
        return new BaseResponse(new StatusResponse(), sysUserinfo);
    }

}