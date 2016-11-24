using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreManager.Application.Interface;
using NetCoreManager.Domain.Entity;
using NetCoreManager.Repository.Interfaces;

namespace NetCoreManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _IUserRepository;

        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            _IUserRepository = userRepository;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _IUserRepository.Get(id).FirstOrDefaultAsync();
            //User user=new User();
            //user.Id = Guid.NewGuid();
            //user.Account = "测试";
            //user.IsDeleted = false;
            //user.CreateTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return user;
        }

        public async Task<User> Login(string account, string pwd)
        {
            //TODO 加密密码

            var result = await _IUserRepository.Login(account, pwd);

            return result;
        }

    }
}
