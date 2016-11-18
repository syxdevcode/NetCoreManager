using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreManager.Application.Interface;
using NetCoreManager.Domain.Entity;

namespace NetCoreManager.Application.Services
{
    public class UserService:IUserService
    {
        public async Task<User> GetById(int id)
        {
            User user=new User();
            user.Id = Guid.NewGuid();
            user.Account = "测试";
            user.IsDeleted = false;
            user.CreateTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return user;
        }

    }
}
