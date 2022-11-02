using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using ClassifiedAds.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public class SysAdminUserRepository : BaseRepository<SysAdminUser>,ISysAdminUserRepository
{
    public SysAdminUserRepository(SaleOrderDBContext db) : base(db)
    {
    }

    public SysAdminUser FindByUserName(string userName)
    {
        return db.SysAdminUser.Where(x => x.Username == userName).FirstOrDefault();
    }
    
    public SysAdminUser FindByUserId(string userId)
    {
        return db.SysAdminUser.Where(x => x.UserId == userId).FirstOrDefault();
    }
    public SysAdminUser FindRawSqlUserName(string userId)
    {
        var blogs = db.SysAdminUser.FromSqlRaw($"Select * from dbo.Sys_Admin_Users where Username = '{userId}'").FirstOrDefault();
        return blogs;
    }

    public SysUserInfo? FindSysUserInfoRawSqlByUserName(string userName)
    {   
       // var a = db.SysUserInfo.FromSqlRaw($"Select SAU.UserId,SAU.Username from dbo.Sys_Admin_Users SAU left join dbo.Sys_Admin_Roles SAR on SAU.UserId  = SAR.RoleId where SAU.UserId = '{userId}'").FirstOrDefault();
        //return a;

      // var blogs = db.SysUserInfo.FromSqlRaw($"Select SAU.UserId,SAU.Username from dbo.Sys_Admin_Users SAU left join dbo.Sys_Admin_Roles SAR on SAU.UserId  = SAR.RoleId where SAU.UserId = '{userName}'").FirstOrDefault();
       
      var result = db.SysUserInfo.FromSqlRaw($"select SAU.UserId,SAU.Username,SAU.Password,SAU.FirstName,SAU.FirstName_EN,SAU.LastName,SAU.LastName_EN,SAU.Language, SAU.Email,SAUP.Gender,SAUP.Phone,SAUP.Mobile,SAU.IsDelete,SAU.IsSuperUser,SAU.isOutSource,SAB.BUCode,SAB.BUName,SAR.RoleName,SAR.isSystemRole from dbo.Sys_Admin_Users SAU left join dbo.Sys_Admin_UserProfile SAUP on SAU.UserId = SAUP.UserId left join dbo.Sys_Admin_UsersInBU SAUIB on SAUP.UserId = SAUIB.UserId left join dbo.Sys_Admin_Bu SAB on SAUIB.BUId = SAB.BUId left join dbo.Sys_Admin_Roles SAR on SAB.RoleId = SAR.RoleId where SAU.ADUser = 'Yes' and SAU.IsDelete = 0 and SAB.isDelete = 0 and SAR.isDelete = 0 and SAU.Username = '{userName}'").FirstOrDefault();
      
      return result;
      
      
      
    }


    public void SaveChange()
    {
        db.SaveChanges();
    }
}