using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreManager.Domain.Entity;

namespace NetCoreManager.Application.Interface
{
    public interface IUserService
    {
        Task<User> GetById(Guid id);
        
        Task<User> Login(string account,string pwd);
    }
}
