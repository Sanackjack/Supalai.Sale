using System.ComponentModel.DataAnnotations;

namespace Spl.Crm.SaleOrder.Modules.Auth.Model;

public class LoginRequest
{
    [Required(ErrorMessage = "username is empty")] 
    public string username { get; set; }
    [Required(ErrorMessage = "password is empty")] 
    public string password { get; set; }

}