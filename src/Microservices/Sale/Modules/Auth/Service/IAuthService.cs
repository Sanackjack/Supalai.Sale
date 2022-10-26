using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using Spl.Crm.SaleOrder.Modules.Auth.Model;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public interface IAuthService
{
    BaseResponse Login(LoginRequest login);
    BaseResponse RefreshToken(string token);
}