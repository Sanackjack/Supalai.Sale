using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassifiedAds.Domain.Constants;

namespace ClassifiedAds.Domain.Entities;

[Table("Sys_Admin_Users", Schema = DBConstant.SaleSchema)]
public class SysAdminUser
{
    [Key]
    public string UserId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FirstName_EN { get; set; }
    public string? LastName_EN { get; set; }
    public string? DisplayName { get; set; }
    public string? DisplayName_EN { get; set; }
    public string? PasswordQuestion { get; set; }
    public string? PasswordAnswer { get; set; }

}
