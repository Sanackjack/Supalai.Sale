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
using ClassifiedAds.Application;

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAppLogger _logger;
    private readonly IStringLocalizer<LocalizeResource> localizer;
    private readonly IAuthService authService;

    public AuthController(IAppLogger _logger,
                            IStringLocalizer<LocalizeResource> localizeResource
                                ,IAuthService authService)
    {
        this._logger = _logger ;
        this.localizer = localizeResource;
        this.authService = authService;
    }

    [HttpGet("get/{id}", Name = "test")]
    public IActionResult test([FromRoute][Required] string id)
    {   // Test design structure
        String result = authService.Login(new LoginRequest());
        return new OkObjectResult(result);
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