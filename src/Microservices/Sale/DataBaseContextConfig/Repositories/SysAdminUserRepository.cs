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
        //var data =db.Database.Sq
      //  var result = db.SysUserInfo.FromSqlRaw("SQL SCRIPT").ToList();
        
        // var overAverageIds = context.Database
        //     .SqlQuery<int>($"SELECT [BlogId] AS [Value] FROM [Blogs]")
        //     .Where(id => id > context.Blogs.Average(b => b.BlogId))
        //     .ToList();
        
        var n = "atgt.sv";
        //var propertyValue = "johndoe";
       // var sql = string.Format($"SELECT * From SysAdminUser Where Username = {0}",propertyName);
       // var blogs = db.SysAdminUser
        //     .FromSqlRaw(sql).FirstOrDefault();
       // db.Database.BeginTransaction()
        var blogs = db.SysAdminUser.FromSqlRaw($"Select * from dbo.Sys_Admin_Users where Username = '{n}'").FirstOrDefault();
        return blogs;
    }

    public SysUserInfo findSysUserInfoRawSqlByUserId(string userId)
    {   var n = "c2";
        // SELECT sau.Username  FROM dbo.Sys_Admin_Users sau 
        // left join dbo.Sys_Admin_Roles sar on sau .UserId  = sar.RoleId 
        // where sau .UserId = 'c2'
       
        // var blogs = db.SysUserInfo.FromSqlRaw($"Select dbo.Sys_Admin_Users.Username from dbo.Sys_Admin_Users where Username = '{n}'").FirstOrDefault();
        // var blogs = db.SysUserInfo.FromSqlRaw($"Select Username from dbo.Sys_Admin_Users  where Username = '{n}'").FirstOrDefault();
       var blogs = db.SysUserInfo.FromSqlRaw($"Select SAU.Username from dbo.Sys_Admin_Users SAU left join dbo.Sys_Admin_Roles SAR on SAU.UserId  = SAR.RoleId where SAU.UserId = '{n}'").FirstOrDefault();

        
        return blogs;
    }


    public void SaveChange()
    {
        db.SaveChanges();
    }
}