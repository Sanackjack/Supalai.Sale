using Microsoft.AspNetCore.Mvc;

namespace Spl.Crm.SaleOrder.Modules;

[Route("spl/api/v{version:apiVersion}")]
public class BaseApiController : ControllerBase
{
    [HttpGet]
    public string GetUserIdFromContext()
    {
        return HttpContext.Items["UserName"] != null ? HttpContext.Items["UserName"].ToString() : "Anonymous";
    }

}