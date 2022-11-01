using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ClassifiedAds.Domain.Entities;

[Table("Sys_Admin_Roles", Schema = "dbo")]
public class SysAdminRole
{
    [Key]
    public string RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
}
