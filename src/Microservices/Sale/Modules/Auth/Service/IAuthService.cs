using Spl.Crm.SaleOrder.Modules.Auth.Model;

namespace Spl.Crm.SaleOrder.Modules.Auth.Service;

public interface IAuthService
{
    string Login(LoginRequest login);
}