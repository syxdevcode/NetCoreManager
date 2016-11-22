using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreManager.Domain.Entity;
using NetCoreManager.Infrastructure.Interfaces;
using NetCoreManager.Repository.Interfaces;

namespace NetCoreManager.Repository
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext) { }
        
    }
}
