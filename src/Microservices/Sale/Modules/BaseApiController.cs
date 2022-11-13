using ClassifiedAds.Infrastructure.JWT;
using Microsoft.AspNetCore.Mvc;

namespace Spl.Crm.SaleOrder.Modules;

[Route("spl/api/v{version:apiVersion}")]
public class BaseApiController : ControllerBase
{
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public string GetUserIdFromContext()
    { 
       TokenInfo? tokenInfo = (TokenInfo)HttpContext.Items["TokenInfo"]!;
       return tokenInfo !=null ? tokenInfo.user_id:"Anonymous";
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public TokenInfo GetTokenInfoFromContext()
    {
        return (TokenInfo)HttpContext.Items["TokenInfo"]!;
    }

}