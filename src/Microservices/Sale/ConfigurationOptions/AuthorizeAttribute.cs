using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.Infrastructure.JWT;
using Microsoft.AspNetCore.Http.Extensions;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
namespace Spl.Crm.SaleOrder.ConfigurationOptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;
        
        //Get Info from context info
        var tokenInfo = (TokenInfo)context.HttpContext.Items["TokenInfo"];
        
        // Check authorization from token Info
        if (tokenInfo == null)
        {
            context.Result = new ObjectResult(new BaseResponse(new StatusResponse(ResponseData.AUTHENTICATION_FAIL.Code, ResponseData.AUTHENTICATION_FAIL.Message)))
            {
                StatusCode = ResponseData.AUTHENTICATION_FAIL.HttpStatus
            };
            return;
        }
        
        // Check authorization refresh token allow api refresh token only
        var url = (string)context.HttpContext.Request.GetEncodedUrl();
        if (!url.Contains("/token/refresh") && "true".Equals(tokenInfo.is_refresh_token) )
        {
            context.Result = new ObjectResult(new BaseResponse(new StatusResponse(ResponseData.TOKEN_INVALID.Code, ResponseData.TOKEN_INVALID.Message)))
            {
                StatusCode = ResponseData.TOKEN_INVALID.HttpStatus
            };
            return;
        }
        
        //manage authorize response message
        ManageErrorTokenEvent(ref context, tokenInfo);

    }

    private static void ManageErrorTokenEvent(ref AuthorizationFilterContext context ,TokenInfo tokenInfo)
    {
        switch (tokenInfo.TokenStatus)
        {
            case TokenStatus.Expire:
                context.Result = new ObjectResult(new BaseResponse(new StatusResponse(ResponseData.TOKEN_EXPIRED.Code, ResponseData.TOKEN_EXPIRED.Message)))
                {
                    StatusCode = ResponseData.TOKEN_EXPIRED.HttpStatus
                };
                break;
            case TokenStatus.Error:
                context.Result = new ObjectResult(new BaseResponse(new StatusResponse(ResponseData.TOKEN_INVALID.Code, ResponseData.TOKEN_INVALID.Message)))
                {
                    StatusCode = ResponseData.TOKEN_INVALID.HttpStatus
                };
                break;
            default:    
            break;
        }

    }

}