using NetCoreManager.Domain;
using NetCoreManager.Infrastructure.Interfaces;
using NetCoreManager.Repository.Interfaces;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NetCoreManager.Repository
{
    public abstract class BaseRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly IQueryable<TAggregateRoot> _entities;
        protected readonly DbSet<TAggregateRoot> _dbSet;

        public BaseRepository(IDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _entities = dbContext.Set<TAggregateRoot>();
        }

        public IQueryable<TAggregateRoot> Get(Guid id)
        {
            return _entities.Where(t => t.Id == id);
        }

        public IQueryable<TAggregateRoot> GetAll()
        {
            return _entities;
        }
    }
}