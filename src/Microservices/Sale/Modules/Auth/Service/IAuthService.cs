using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using Spl.Crm.SaleOrder.Modules.Auth.Model;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public interface IAuthService
{
    BaseResponse Login(LoginRequest login);
    BaseResponse RefreshToken(TokenInfo token);
    BaseResponse getUser();
}