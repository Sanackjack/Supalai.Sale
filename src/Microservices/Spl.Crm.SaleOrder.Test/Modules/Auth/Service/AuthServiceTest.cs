
using Moq;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using ClassifiedAds.Infrastructure.JWT;
using ClassifiedAds.Infrastructure.LDAP;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;
using ClassifiedAds.Domain.Uow;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Spl.Crm.SaleOrder.Cache.Redis.Service;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;
using Spl.Crm.SaleOrder.Modules.Auth.Service;

namespace Spl.Crm.SaleOrder.Test.Modules.Auth.Service;

public class AuthServiceTest
{
    private AuthService _service;
    private Mock<IJwtUtils> _jwtUtilsMock;
    private Mock<ILDAPUtils> _ldapUtilsMock;
    private Mock<IAppLogger> _loggerMock;
    private Mock<IConfiguration> _configurationMock;
    private Mock<ISysAdminUserRepository> _sysAdminUserRepositoryMock;
    private Mock<ISysAdminRoleRepository> _sysAdminRoleRepositoryMock;
    private Mock<ISaleOrderRepository> _saleOrderRepositoryMock;
    private Mock<IUnitOfWork> _uowMock;
    private Mock<IUserCacheService> _userCacheServiceMock;
    public AuthServiceTest()
    {
        _jwtUtilsMock = new Mock<IJwtUtils>();
        _ldapUtilsMock = new Mock<ILDAPUtils>();
        _loggerMock = new Mock<IAppLogger>();
        _configurationMock = new Mock<IConfiguration>();
        _sysAdminUserRepositoryMock = new Mock<ISysAdminUserRepository>();
        _sysAdminRoleRepositoryMock = new Mock<ISysAdminRoleRepository>();
        _saleOrderRepositoryMock = new Mock<ISaleOrderRepository>();
        _userCacheServiceMock = new Mock<IUserCacheService>();
        _uowMock = new Mock<IUnitOfWork>();
        _service = new AuthService(_configurationMock.Object, _jwtUtilsMock.Object,
            _ldapUtilsMock.Object, _sysAdminUserRepositoryMock.Object, _uowMock.Object,
            _sysAdminRoleRepositoryMock.Object,
            _saleOrderRepositoryMock.Object, _loggerMock.Object,
            _userCacheServiceMock.Object);

        //arrange config global
        _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "JwtSettings:Expire")]).Returns("2");   
    }
    
    [Fact]
    public void Test_Case_Login_AllValid_Success()
    {
        //arrange
        _ldapUtilsMock.Setup(p => p.CheckUserLoginLdap(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        SysUserInfo sysUserInfo = new SysUserInfo()
        {
            UserId = "supachaisri",
            FirstName = "supachai",
            LastName = "supachaisri",
            Email = "spl@gmail.com",
            RoleName = "admin"
        };
        
        List<SysUserInfo> listSysUserInfo = new List<SysUserInfo>();
        listSysUserInfo.Add(sysUserInfo);
        _saleOrderRepositoryMock.Setup(p => p.FindSysUserInfoRawSqlByUserName(It.IsAny<string>())).Returns(listSysUserInfo);

        _jwtUtilsMock.Setup(p => p.GenerateJwtToken(It.IsAny<TokenInfo>())).Returns("token1234567890.payload.signature");
        _jwtUtilsMock.Setup(p => p.GenerateRefreshToken(It.IsAny<TokenInfo>())).Returns("refresh_token1234567890.payload.signature");
        
        
        //act
        var result = _service.Login(new LoginRequest()
        {
            username = "supachai",
            password = "password"
        });

        //assert
        Assert.NotNull(result);
        Assert.IsType<BaseResponse>(result);

        var data = result?.data as LoginResponse;
        Assert.NotNull(data);
        Assert.Equal("token1234567890.payload.signature", data?.token);
        Assert.Equal("refresh_token1234567890.payload.signature", data?.refresh_token);
        Assert.NotNull(data?.user_info);
        
        //Verify
        _userCacheServiceMock.Verify(p => p.Delete(It.IsAny<string>()),Times.Once);
        _userCacheServiceMock.Verify(p => p.Set(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<int>(),It.IsAny<int>()),Times.Once);

    }
    
    [Fact]
    public void Test_Case_Login_UserNotFound_InLDAP_Fail()
    {
        //arrange
        _ldapUtilsMock.Setup(p => p.CheckUserLoginLdap(It.IsAny<string>(), It.IsAny<string>())).Throws(new AuthenicationErrorException(ResponseData.INCORRECT_USERNAME_PASSWORD));
       
        //act
        try
        {
            var result = _service.Login(new LoginRequest()
            {
                username = "invalid",
                password = "password"
            });
        }
        catch (Exception e)
        {
         //assert
         Assert.IsType<AuthenicationErrorException>(e);
         var error = e as AuthenicationErrorException;
         Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.Message, error?.message);
         Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.Code, error?.code);
         Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.HttpStatus, error?.httpStatus);
        }
        //Verify
        _saleOrderRepositoryMock.Verify(m => m.FindSysUserInfoRawSqlByUserName("invalid"),Times.Never);
        _jwtUtilsMock.Verify(p => p.GenerateJwtToken(It.IsAny<TokenInfo>()),Times.Never);
        _jwtUtilsMock.Verify(p => p.GenerateRefreshToken(It.IsAny<TokenInfo>()),Times.Never);
    }
    
    [Fact]
    public void Test_Case_Login_UserNotFound_InDB_Fail()
    {
        //arrange
        _ldapUtilsMock.Setup(p => p.CheckUserLoginLdap(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _saleOrderRepositoryMock.Setup(p => p.FindSysUserInfoRawSqlByUserName(It.IsAny<string>()))
            .Returns(null as List<SysUserInfo>);
 
        //act
        try
        {
            var result = _service.Login(new LoginRequest()
            {
                username = "invalid",
                password = "password"
            });
        }
        catch (Exception e)
        {
            //assert
            Assert.IsType<AuthenicationErrorException>(e);
            var error = e as AuthenicationErrorException;
            Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.Message, error?.message);
            Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.Code, error?.code);
            Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.HttpStatus, error?.httpStatus);
        }
        
        //Verify
        _jwtUtilsMock.Verify(p => p.GenerateJwtToken(It.IsAny<TokenInfo>()),Times.Never);
        _jwtUtilsMock.Verify(p => p.GenerateRefreshToken(It.IsAny<TokenInfo>()),Times.Never);

    }
    
    [Fact]
    public void Test_Case_RefreshToken_AllValid_Success()
    {
        //arrange
        SysUserInfo sysUserInfo = new SysUserInfo()
        {
            UserId = "supachaisri",
            FirstName = "supachai",
            LastName = "supachaisri",
            Email = "spl@gmail.com",
            RoleName = "admin"
        };
        List<SysUserInfo> listSysUserInfo = new List<SysUserInfo>();
        listSysUserInfo.Add(sysUserInfo);
        _saleOrderRepositoryMock.Setup(p => p.FindSysUserInfoRawSqlByUserName(It.IsAny<string>())).Returns(listSysUserInfo);

        _jwtUtilsMock.Setup(p => p.GenerateJwtToken(It.IsAny<TokenInfo>())).Returns("token1234567890.payload.signature");
        _jwtUtilsMock.Setup(p => p.GenerateRefreshToken(It.IsAny<TokenInfo>())).Returns("refresh_token1234567890.payload.signature");
        
        //act
        var result = _service.RefreshToken(new TokenInfo());

        //assert
        Assert.NotNull(result);
        Assert.IsType<BaseResponse>(result);

        var data = result?.data as RefreshTokenResponse;
        Assert.NotNull(data);
        Assert.Equal("token1234567890.payload.signature", data?.token);
        Assert.Equal("refresh_token1234567890.payload.signature", data?.refresh_token);
        //Verify
        _userCacheServiceMock.Verify(p => p.Delete(It.IsAny<string>()),Times.Once);
        _userCacheServiceMock.Verify(p => p.Set(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<int>(),It.IsAny<int>()),Times.Once);

    }
    
    [Fact]
    public void Test_Case_RefreshToken_UserNotFound_InDB_Fail()
    {
        //arrange
        _ldapUtilsMock.Setup(p => p.CheckUserLoginLdap(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _saleOrderRepositoryMock.Setup(p => p.FindSysUserInfoRawSqlByUserName(It.IsAny<string>()))
            .Returns(null as List<SysUserInfo>);
 
        //act
        try
        {
            _service.RefreshToken(new TokenInfo());
        }
        catch (Exception e)
        {
            //assert
            Assert.IsType<AuthenicationErrorException>(e);
            var error = e as AuthenicationErrorException;
            Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.Message, error?.message);
            Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.Code, error?.code);
            Assert.Equal(ResponseData.INCORRECT_USERNAME_PASSWORD.HttpStatus, error?.httpStatus);
        }
    }
}