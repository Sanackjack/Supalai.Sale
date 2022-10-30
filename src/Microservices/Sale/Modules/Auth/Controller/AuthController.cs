using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
using ClassifiedAds.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Spl.Crm.SaleOrder;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly IStringLocalizer<AuthController> stringLocalizer;

    private readonly IStringLocalizer<SharedResource> sharedResourceLocalizer;

    public AuthController(IAuthService authService, IStringLocalizer<AuthController> postsControllerLocalizer,
        IStringLocalizer<SharedResource> sharedResourceLocalizer)
    {
        this.authService = authService;
        this.stringLocalizer = postsControllerLocalizer;

        this.sharedResourceLocalizer = sharedResourceLocalizer;
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

        var article = sharedResourceLocalizer["Article"];
        var postName = sharedResourceLocalizer.GetString("Welcome").Value ?? "";

        return Ok(new { PostType = article.Value, PostName = postName });
    }

}