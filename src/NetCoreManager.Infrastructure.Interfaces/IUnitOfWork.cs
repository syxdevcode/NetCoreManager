using System.Threading;
using System.Threading.Tasks;

namespace NetCoreManager.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        void RegisterNew<TEntity>(TEntity entity)
               where TEntity : class;

        void RegisterDirty<TEntity>(TEntity entity)
            where TEntity : class;

        void RegisterClean<TEntity>(TEntity entity)
            where TEntity : class;

        void RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : class;

        Task<bool> CommitAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

        void Rollback();
    }
}