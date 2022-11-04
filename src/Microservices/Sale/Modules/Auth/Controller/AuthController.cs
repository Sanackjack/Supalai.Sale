using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
using ClassifiedAds.Infrastructure.Localization;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Spl.Crm.SaleOrder;
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
    private readonly IStringLocalizer<LocalizeResource> localizer;
    public AuthController(IAppLogger _logger,
                            IStringLocalizer<LocalizeResource> localizeResource
                                ,IAuthService authService)
    {
        this._logger = _logger ;
        this.localizer = localizeResource;
        this._authservice = authService;
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

    [HttpGet]
    [Route("localize")]
    public IActionResult localize()
    {

        var article = localizer["Article"]; 
        _logger.Debug("Hello from GetLog");
        return Ok(new { PostType = article.Value });
    }
}