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
        public async Task<User> GetById(int Id)
        {
            User user=new User();
            user.Id = 1;
            user.Name = "测试";
            user.IsDelete = false;
            user.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return user;
        }

    }
}
