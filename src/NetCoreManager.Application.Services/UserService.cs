using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreManager.Application.Interface;
using NetCoreManager.Domain.Entity;
using NetCoreManager.Infrastructure;
using NetCoreManager.Infrastructure.UnitOfWork;

namespace NetCoreManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<ManagerDbContext> _unitOfWork;

        public UserService(IUnitOfWork<ManagerDbContext> unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().FindAsync(id);
            return user;
        }

        public async Task<User> Login(string account, string pwd)
        {
            var result = await _unitOfWork.Repository<User>().Where(o => o.Account == account && o.Password == pwd).FirstOrDefaultAsync();

            return result;
        }

    }
}
