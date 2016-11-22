using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NetCoreManager.Application.Interface;
using NetCoreManager.Application.Services;
using NetCoreManager.Infrastructure.Interfaces;
using NetCoreManager.Repository;
using NetCoreManager.Repository.Interfaces;

namespace NetCoreManager.Infrastructure.IoC
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<ManagerDbContext>().As<IDbContext>();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}
