using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;


namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[Route("/spl/api/v1/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authservice;
    public AuthController(IAuthService authService)
    {
        this._authservice = authService;
    }

    [HttpPost("authentication")]
    public IActionResult Login([FromBody][Required]LoginRequest account)
    {   
        var result = _authservice.Login(new LoginRequest());
        return new OkObjectResult(result);
    }
}