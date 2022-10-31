using System.Linq.Expressions;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.DataBaseContextConfig;

namespace Spl.Crm.SaleOrder.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly SaleOrderDbContext db;
    public BaseRepository(SaleOrderDbContext context)
    {
        db = context;
    }

    public void Add(T entity)
    {
        db.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        db.Set<T>().AddRange(entities);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return db.Set<T>().Where(expression);
    }

    public IEnumerable<T> GetAll()
    {
        return db.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return db.Set<T>().Find(id);
    }

    public void Remove(T entity)
    {
        db.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        db.Set<T>().RemoveRange(entities);
    }
}