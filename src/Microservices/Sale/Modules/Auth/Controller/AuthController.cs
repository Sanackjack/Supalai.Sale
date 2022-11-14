using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
using ClassifiedAds.Infrastructure.Localization;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Spl.Crm.SaleOrder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Spl.Crm.SaleOrder.Cache.Redis.Service;
using Spl.Crm.SaleOrder.Cache.Redis.Service.implement;

using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using ClassifiedAds.Application;
using ClassifiedAds.Infrastructure.ValidateModelAttribute;

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[ApiController]
[Authorize]
public class AuthController : BaseApiController
{

    private readonly IAuthService _authservice;
    private readonly IAppLogger _logger;
    private readonly IStringLocalizer<LocalizeResource> _localizer;
    private readonly IUserCacheService _userCacheService;

    public AuthController(IAppLogger _logger,
                            IStringLocalizer<LocalizeResource> localizeResource,
                            IAuthService authService,
                            IUserCacheService userCacheService)
    {
        this._logger = _logger ;
        this._logger = _logger ;
        this._localizer = localizeResource;
        this._authservice = authService;
        this._userCacheService = userCacheService;
    }
    
    [AllowAnonymous]
    [ValidateModel]
    [HttpPost("authentication")]
    public IActionResult Login([FromBody][Required]LoginRequest account)
    { 
        return new OkObjectResult(_authservice.Login(account));
    }

    [HttpGet("token/refresh")]
    public IActionResult refreshToken()
    {
        var token = GetTokenInfoFromContext();
        
        _logger.Info(string.Format("Username is {0} Call Refresh Token",token.username));
        
        return new OkObjectResult(_authservice.RefreshToken(token));
    }
}