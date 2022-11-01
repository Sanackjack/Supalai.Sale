using System;

namespace ClassifiedAds.Domain.Uow;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();
    void Dispose();
}
