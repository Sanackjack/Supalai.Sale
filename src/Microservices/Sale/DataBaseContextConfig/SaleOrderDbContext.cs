using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.Entities;
using System.Reflection;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig;

public class SaleOrderDbContext : DbContext
{
    public SaleOrderDbContext()
    {
    }
    public SaleOrderDbContext(DbContextOptions<SaleOrderDbContext> options) : base(options)
    {
    }
    
    public DbSet<SysAdminUser> SysAdminUser { get; set; }
    
    
    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);
    //     builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<SysAdminUser>()
        //     .Property(e => e.UserId)
        //     .IsUnicode(false);
    }
}