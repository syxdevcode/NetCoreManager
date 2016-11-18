using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreManager.Domain;

namespace NetCoreManager.Repository.Interfaces
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot:class,IAggregateRoot
    {
        IQueryable<TAggregateRoot> Get(Guid id);

        IQueryable<TAggregateRoot> GetAll();
    }
}
