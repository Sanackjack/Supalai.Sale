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
                                ,IAuthService authService, IAuthService authservice)
    {
        this._logger = _logger ;
        this.localizer = localizeResource;
        _authservice = authservice;
        this._authservice = authService;
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
}