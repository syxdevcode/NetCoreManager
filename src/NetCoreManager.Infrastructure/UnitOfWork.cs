using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreManager.Infrastructure.Interfaces;

namespace NetCoreManager.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private bool disposed = false;

        private UnitOfWork(IDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public void RegisterNew<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void RegisterDirty<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().Update(entity);
            //_dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void RegisterClean<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Unchanged;
        }

        public void RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<bool> CommitAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken) > 0;
        }



        public void Rollback()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose the db context.
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }
    }
}
