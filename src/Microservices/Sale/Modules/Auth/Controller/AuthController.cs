using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;
using Spl.Crm.SaleOrder.ConfigurationOptions;

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[Route("/spl/api/v1/")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authservice;
    public AuthController(IAuthService authService)
    {
        this._authservice = authService;
    }
    [AllowAnonymous]
    [HttpPost("authentication")]
    public IActionResult Login([FromBody][Required]LoginRequest account)
    {
        var result = _authservice.Login(account);
        return new OkObjectResult(result);
        
        // var result = _authservice.Login(account);
        // return new OkObjectResult(result);
    }
    
    [HttpGet("token/refresh")]
    public IActionResult refreshToken()
    {
        string userId = HttpContext.Items["UserName"].ToString();
        var response = _authservice.RefreshToken(userId);
        return new OkObjectResult(response);
    }
}