
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
    public AuthServiceTest()
    {
        _jwtUtilsMock = new Mock<IJwtUtils>();
        _ldapUtilsMock = new Mock<ILDAPUtils>();
        _loggerMock = new Mock<IAppLogger>();
        _configurationMock = new Mock<IConfiguration>();
        _sysAdminUserRepositoryMock = new Mock<ISysAdminUserRepository>();
        _sysAdminRoleRepositoryMock = new Mock<ISysAdminRoleRepository>();
        _saleOrderRepositoryMock = new Mock<ISaleOrderRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _service = new AuthService(_configurationMock.Object, _jwtUtilsMock.Object,
            _ldapUtilsMock.Object, _sysAdminUserRepositoryMock.Object, _uowMock.Object,
            _sysAdminRoleRepositoryMock.Object,
            _saleOrderRepositoryMock.Object, _loggerMock.Object);

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

        _jwtUtilsMock.Setup(p => p.GenerateJwtToken(It.IsAny<TokenInfo>())).Returns("token1234567890");
        _jwtUtilsMock.Setup(p => p.GenerateRefreshToken(It.IsAny<TokenInfo>())).Returns("refresh_token1234567890");
        
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
        Assert.Equal("token1234567890", data?.token);
        Assert.Equal("refresh_token1234567890", data?.refresh_token);
        Assert.NotNull(data?.user_info);
    }
}