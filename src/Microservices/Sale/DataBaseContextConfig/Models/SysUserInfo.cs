namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

public class SysUserInfo
{
 
   public string? UserId { get; set; }
   public string? Username { get; set; }
   public string? Password { get; set; }
   public string? FirstName { get; set; }
   public string? FirstName_EN { get; set; }
   public string? LastName { get; set; }
   public string? LastName_EN { get; set; }
   public string? Language { get; set; }
   public string? Email { get; set; }
   public string? Gender { get; set; }
   public string? Phone { get; set; }
   public string? Mobile { get; set; }
   public bool IsDelete { get; set; }
   public bool IsSuperUser { get; set; }
   public bool isOutSource { get; set; }
   public string? BUCode { get; set; }
   public string? BUName { get; set; }
   public string? RoleName { get; set; }
   public bool isSystemRole { get; set; }
}