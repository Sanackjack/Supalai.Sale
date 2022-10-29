using System.Linq;
using System.Threading.Tasks;
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

        var tokenInfo = jwtUtils.ValidateJwtToken(token);
        if (tokenInfo != null)
        {
            context.Items["TokenInfo"] = tokenInfo;
        }

        await _next(context);
    }
}
