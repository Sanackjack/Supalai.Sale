namespace Spl.Crm.SaleOrder.Uow;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();
    void Dispose();
}