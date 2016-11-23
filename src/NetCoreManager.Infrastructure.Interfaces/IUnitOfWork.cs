using System.Linq;
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

        //int ExecuteSqlCommand(string sql, params object[] parameters);

        //IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;

        Task<bool> CommitAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

        void Rollback();
    }
}