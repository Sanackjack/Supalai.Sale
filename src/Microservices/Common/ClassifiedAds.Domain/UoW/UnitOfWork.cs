using System;
using Microsoft.EntityFrameworkCore;
namespace ClassifiedAds.Domain.Uow;

public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public UnitOfWork(TDbContext context)
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));
    }


    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
    
}
