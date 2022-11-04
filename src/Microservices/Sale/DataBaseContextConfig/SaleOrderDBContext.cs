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
   // public DbSet<Roles> RoleName { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SysUserInfo>()
            // .Property(m => m.RoleName).HasColumnType("RoleName")
            // ;
            .HasNoKey();
          //  .HasMany<Roles>(x => x.RoleName);
            //.HasAlternateKey(x => x.RoleName);
            
            // .HasNoKey()
            // .HasMany(c => c.RoleName);
            //.WithOne(s => s.RoleName);
        
        // modelBuilder.Entity<SysUserInfo>(
        //     eb =>
        //     {
        //         eb.HasNoKey();
        //      //   eb
        //     }
        // );
        //modelBuilder.Entity<Roles>().HasNoKey();
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        // modelBuilder.Entity<SysAdminUser>()
        //     .Property(e => e.UserId)
        //     .IsUnicode(false);
    }
}