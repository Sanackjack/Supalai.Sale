using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassifiedAds.Domain.Constants;

namespace ClassifiedAds.Domain.Entities;

[Table("Sys_Admin_Roles", Schema = DBConstant.SaleSchema)]
public class SysAdminRole
{
    [Key]
    public string RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
}
