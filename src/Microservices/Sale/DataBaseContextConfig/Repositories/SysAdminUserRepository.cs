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

    public SysUserInfo findSysUserInfoRawSqlByUserId(string userId)
    {   
       var blogs = db.SysUserInfo.FromSqlRaw($"Select SAU.UserId,SAU.Username from dbo.Sys_Admin_Users SAU left join dbo.Sys_Admin_Roles SAR on SAU.UserId  = SAR.RoleId where SAU.UserId = '{userId}'").FirstOrDefault();
       return blogs;
    }


    public void SaveChange()
    {
        db.SaveChanges();
    }
}