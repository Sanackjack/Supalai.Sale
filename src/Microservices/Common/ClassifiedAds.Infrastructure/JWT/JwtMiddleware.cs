using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
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
        
        Console.WriteLine("link"+context.Request.GetEncodedUrl());
        Console.WriteLine("link"+context.Request.GetDisplayUrl());

        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            Console.WriteLine("user"+userId);
            context.Items["UserName"] = userId;
        }

        await _next(context);
    }
}
