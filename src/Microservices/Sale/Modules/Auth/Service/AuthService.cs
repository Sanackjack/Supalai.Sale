using System.Transactions;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using Novell.Directory.Ldap;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.Entities;
using Spl.Crm.SaleOrder.Repositories;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public class AuthService : IAuthService
{
    private IJwtUtils _jwtUtils;
    private ILDAPUtils _ldapUtils;
    private readonly IConfiguration _configuration;
    private readonly ISysAdminUserRepository _sysAdminUserRepository;
    
    public AuthService(IConfiguration configuration,IJwtUtils jwtUtils, ILDAPUtils ldapUtils,ISysAdminUserRepository sysAdminUserRepository)
    {
        _configuration = configuration;
        _jwtUtils = jwtUtils;
        _ldapUtils = ldapUtils;
        _sysAdminUserRepository = sysAdminUserRepository;
    }

    public BaseResponse Login(LoginRequest login)
    {
        // validate account LDAP
        // _ldapUtils.CheckUserLoginLdap(login.username, login.password);
        
       // Query Data SPLDB Get info and role to userInfo
       try
       {
           using (TransactionScope scope = new TransactionScope())
           {
               SysAdminUser sysAdminUser2 = new SysAdminUser();
               sysAdminUser2.UserId = login.username;
               sysAdminUser2.Password = "test";
               _sysAdminUserRepository.Add(sysAdminUser2);
               _sysAdminUserRepository.Save();
               scope.Complete();
           }
       }
       catch (Exception e)
       {
           Console.WriteLine("error scope");
           throw;
       }
       SysAdminUser sysAdminUser = _sysAdminUserRepository.FindByUserName(login.username);
        
        UserInfo userInfo = new UserInfo();
        userInfo.firstname = "firstname";
        userInfo.lastname = "firstname";
        userInfo.email = "spl@gmail.com";
        userInfo.user_id = "supachai01";
        userInfo.username = "supachai";
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
        userInfo.firstname = "firstname";
        userInfo.lastname = "firstname";
        userInfo.email = "spl@gmail.com";
        userInfo.user_id = "supachai01";
        userInfo.username = "supachai";
        userInfo.role_name = new string[]{"admin","user"};
        
        
        RefreshTokenResponse response = new RefreshTokenResponse()
        {
            token = _jwtUtils.GenerateJwtToken(userInfo.user_id),
            refresh_token = _jwtUtils.GenerateRefreshToken(userInfo.user_id)
        };
        return new BaseResponse(new StatusResponse(), response);
    }
}