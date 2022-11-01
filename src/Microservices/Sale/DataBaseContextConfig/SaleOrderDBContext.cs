using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.Entities;
using System.Reflection;

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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<SysAdminUser>()
        //     .Property(e => e.UserId)
        //     .IsUnicode(false);
    }
}