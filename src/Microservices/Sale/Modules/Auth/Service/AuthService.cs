using System.Transactions;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using ClassifiedAds.Domain.Entities;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using Novell.Directory.Ldap;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;
using ClassifiedAds.Domain.Uow;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private ILDAPUtils _ldapUtils;
    private readonly IConfiguration _configuration;
    private readonly ISysAdminUserRepository _sysAdminUserRepository;
    private readonly ISysAdminRoleRepository _sysAdminRoleRepository;
    private readonly IUnitOfWork _uow;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils, ILDAPUtils ldapUtils,ISysAdminUserRepository sysAdminUserRepository, IUnitOfWork uow, ISysAdminRoleRepository sysAdminRoleRepository)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
        _ldapUtils = ldapUtils;
        _sysAdminUserRepository = sysAdminUserRepository;
        _uow = uow;
        _sysAdminRoleRepository = sysAdminRoleRepository;
    }

    public BaseResponse Login(LoginRequest login)
    {
        // validate account LDAP
        // _ldapUtils.CheckUserLoginLdap(login.username, login.password);
        
       //check user should has in DB
       // SysAdminUser sysAdminUser = _sysAdminUserRepository.FindByUserName(login.username);
       // if (sysAdminUser == null)
       //     throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
       //
       
       // Query Data SPLDB Get info and role to userInfo
 //      SysUserInfo sysUserinfo = _sysAdminUserRepository.findSysUserInfoRawSqlByUserId(login.username);
       UserInfo userInfo = new UserInfo();
        userInfo.firstname = "firstname";
        userInfo.lastname = "firstname";
        userInfo.email = "spl@gmail.com";
        userInfo.user_id = "supachai01";
        userInfo.username = "supachai";
       // userInfo.role_name = new string[]{""};
        // build token
        
        TokenInfo tokenInfo = new TokenInfo()
        {
            user_id = userInfo.user_id,
            firstname = userInfo.firstname,
            lastname = userInfo.lastname,
            email = userInfo.email,
            username = userInfo.username,
            role = userInfo.role_name
        };
        
        LoginResponse response = new LoginResponse();
        response.token = _jwtUtils.GenerateJwtToken(tokenInfo);;
        response.refresh_token = _jwtUtils.GenerateRefreshToken(tokenInfo);;
        response.user_info = userInfo;
        
        
        //terminate old session in redis
        return new BaseResponse(new StatusResponse(), response);
    }

    public BaseResponse RefreshToken(string userId)
    {
        // Query Data SPLDB Get info and role to userInfo
        UserInfo userInfo = new UserInfo();
        userInfo.firstname = "firstname";
        userInfo.lastname = "firstname";
        userInfo.email = "spl@gmail.com";
        userInfo.user_id = "supachai01";
        userInfo.username = "supachai";
        userInfo.role_name = new string[]{"admin","user"};


        TokenInfo tokenInfo = new TokenInfo()
        {
            user_id = userInfo.user_id,
            firstname = userInfo.firstname,
            lastname = userInfo.lastname,
            email = userInfo.email,
            username = userInfo.username,
            role = userInfo.role_name
        };
        
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(tokenInfo),
            refresh_token = _jwtUtils.GenerateRefreshToken(tokenInfo)
        };
        return new BaseResponse(new StatusResponse(), response);
    }
}