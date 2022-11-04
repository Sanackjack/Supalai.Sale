using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;
using ClassifiedAds.Domain.Uow;
using ClassifiedAds.Infrastructure.Logging;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private ILDAPUtils _ldapUtils;
    private readonly IAppLogger _logger;
    private readonly IConfiguration _configuration;
    private readonly ISysAdminUserRepository _sysAdminUserRepository;
    private readonly ISysAdminRoleRepository _sysAdminRoleRepository;
    private readonly ISaleOrderRepository _saleOrderRepository;
    private readonly IUnitOfWork _uow;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils, ILDAPUtils ldapUtils,ISysAdminUserRepository sysAdminUserRepository, IUnitOfWork uow, ISysAdminRoleRepository sysAdminRoleRepository, ISaleOrderRepository saleOrderRepository, IAppLogger logger)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
        _ldapUtils = ldapUtils;
        _sysAdminUserRepository = sysAdminUserRepository;
        _sysAdminRoleRepository = sysAdminRoleRepository;
        _saleOrderRepository = saleOrderRepository;
        _logger = logger;
        _uow = uow;
    }

    public BaseResponse Login(LoginRequest login)
    {
        _logger.Info("Authentication LDAP"); 
       // _ldapUtils.CheckUserLoginLdap(login.username, login.password);
       
       _logger.Info("Authentication DataBase");
       var sysUserinfo = _saleOrderRepository.FindSysUserInfoRawSqlByUserName(login.username);
       if (sysUserinfo == null)
            throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
       
       UserInfo userInfo = BuildUserInfo(sysUserinfo);
       
       _logger.Info("Build JWT Token");
        TokenInfo tokenInfo = BuildTokenInfo(userInfo);
        LoginResponse response = new LoginResponse();
        response.token = _jwtUtils.GenerateJwtToken(tokenInfo);;
        response.refresh_token = _jwtUtils.GenerateRefreshToken(tokenInfo);;
        response.user_info = userInfo;
        
        _logger.Info("Create new Session In Redis");
        //terminate old session in redis
        
        return new BaseResponse(new StatusResponse(), response);
    }

    public BaseResponse RefreshToken(TokenInfo token)
    {
        _logger.Info("Authentication DataBase");
        var sysUserinfo = _saleOrderRepository.FindSysUserInfoRawSqlByUserName(token.username);
        if (sysUserinfo == null)
            throw new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD);
        UserInfo userInfo = BuildUserInfo(sysUserinfo);
        
        _logger.Info("Build JWT Token");
        TokenInfo tokenInfo = BuildTokenInfo(userInfo);
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(tokenInfo),
            refresh_token = _jwtUtils.GenerateRefreshToken(tokenInfo)
        };
        
        _logger.Info(string.Format("Create new Session In Redis of username {0}",token.username));
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
    
    private UserInfo BuildUserInfo(List<SysUserInfo> sysUserinfo)
    {
        return   new UserInfo()
        {
            firstname = sysUserinfo[0].FirstName,
            lastname = sysUserinfo[0].LastName,
            email = sysUserinfo[0].Email,
            user_id = sysUserinfo[0].UserId,
            username = sysUserinfo[0].Username,
            role_name = sysUserinfo.Select(s => { return s.RoleName;}).ToList()
        };
    }
}