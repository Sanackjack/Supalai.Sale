using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassifiedAds.Infrastructure.JWT;


public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var tokenInfo = jwtUtils.ValidateJwtToken(token);
        if (tokenInfo != null)
        {
            context.Items["TokenInfo"] = tokenInfo;
        }

        await _next(context);
    }
}
