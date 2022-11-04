using Microsoft.EntityFrameworkCore;
using ClassifiedAds.Domain.Entities;
using System.Reflection;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig;

public class SaleOrderDBContext : DbContext
{
    public SaleOrderDBContext()
    {
    }
    public SaleOrderDBContext(DbContextOptions<SaleOrderDBContext> options) : base(options)
    {
    }
    
    public DbSet<SysAdminUser> SysAdminUser { get; set; }
    public DbSet<SysAdminRole> SysAdminRole { get; set; }
    
    
    //customModel
    public DbSet<SysUserInfo> SysUserInfo { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SysUserInfo>()
            .HasNoKey();

        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}