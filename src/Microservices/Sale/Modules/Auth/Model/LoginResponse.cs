using ClassifiedAds.Infrastructure.JWT;

namespace Spl.Crm.SaleOrder.Modules.Auth.Model;

public class LoginResponse
{
    public string token { get; set; }
    public string refresh_token { get; set; }
    public UserInfo user_info { get; set; }
}