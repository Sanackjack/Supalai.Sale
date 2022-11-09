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

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[ApiController]
[Authorize]
public class AuthController : BaseApiController
{

    private readonly IAuthService _authservice;
    private readonly IAppLogger _logger;
    private readonly IStringLocalizer<LocalizeResource> localizer;
    private IMasterConfigCacheService masterConfigCache;
    private IUserCacheService userCacheService;
    public AuthController(IAppLogger _logger,
                            IStringLocalizer<LocalizeResource> localizeResource
                                ,IAuthService authService, IAuthService authservice
                                    IMasterConfigCacheService masterConfigCache,
                                        IUserCacheService userCacheService)
    {
        this._logger = _logger ;
        this.localizer = localizeResource;
        _authservice = authservice;
        this._authservice = authService;
        this.masterConfigCache = masterConfigCache;
        this.userCacheService = userCacheService;
    }
    
    [AllowAnonymous]
    [HttpPost("authentication")]
    public IActionResult Login([FromBody][Required]LoginRequest account)
    {
        var result = _authservice.Login(account);
        return new OkObjectResult(result);
    }

    [HttpGet("token/refresh")]
    public IActionResult refreshToken()
    {
        var token = GetTokenInfoFromContext();
        var response = _authservice.RefreshToken(token);
        return new OkObjectResult(response);
    }

    [HttpGet]
    [Route("localize")]
    public IActionResult localize()
    {

        var article = localizer["Article"]; 
        _logger.Debug("Hello from GetLog");
        return Ok(new { PostType = article.Value });
    }
    
    [HttpGet("redis")]
    public IActionResult addRedisData()
    {

         var val = userCacheService.Get<string>("UserCacheService");
         //userCacheService.Set("UserCacheService", "value");
         //userCacheService.Refresh("UserCacheService");


        return new OkObjectResult("okay");
    }
}