using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories.Impl;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public class SaleOrderRepository : ISaleOrderRepository
{
   private readonly SaleOrderDBContext _db;
   
   public SaleOrderRepository(SaleOrderDBContext saleOrderDb)
   {
      this._db = saleOrderDb;
   }
   
   public List<SysUserInfo>? FindSysUserInfoRawSqlByUserName(string userName)
   {
      List<SysUserInfo>? sysUserInfos = _db.SysUserInfo.FromSqlRaw(
            $"select SAU.UserId,SAU.Username,SAU.Password,SAU.FirstName,SAU.FirstName_EN,SAU.LastName,SAU.LastName_EN,SAU.Language, SAU.Email,SAUP.Gender,SAUP.Phone,SAUP.Mobile,SAU.IsDelete,SAU.IsSuperUser,SAU.isOutSource,SAB.BUCode,SAB.BUName,SAR.RoleName,SAR.isSystemRole from dbo.Sys_Admin_Users SAU left join dbo.Sys_Admin_UserProfile SAUP on SAU.UserId = SAUP.UserId left join dbo.Sys_Admin_UsersInBU SAUIB on SAUP.UserId = SAUIB.UserId left join dbo.Sys_Admin_Bu SAB on SAUIB.BUId = SAB.BUId left join dbo.Sys_Admin_Roles SAR on SAB.RoleId = SAR.RoleId where SAU.ADUser = 'Yes' and SAU.IsDelete = 0 and SAB.isDelete = 0 and SAR.isDelete = 0 and SAU.Username = '{userName}'").OrderByDescending(x=>x.UserId).ToList();

      return sysUserInfos.Count != 0 ? sysUserInfos : null;

   }
}