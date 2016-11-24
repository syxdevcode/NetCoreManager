using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreManager.Domain.Entity;
using NetCoreManager.Infrastructure.Interfaces;
using NetCoreManager.Repository.Interfaces;

namespace NetCoreManager.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext) { }

        public async Task<User> Login(string account, string pwd)
        {
            var result = await _entities.Where(o => o.Account == account && o.Password == pwd).FirstOrDefaultAsync();
            return result;
        }


    }
}
