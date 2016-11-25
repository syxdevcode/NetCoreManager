using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreManager.Domain.Entity;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace NetCoreManager.Infrastructure
{
    public class ManagerDbContext: DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //UserRole关联配置
            builder.Entity<UserRole>()
              .HasKey(ur => new { ur.UserId, ur.RoleId });

            //RoleMenu关联配置
            builder.Entity<RoleMenu>()
              .HasKey(rm => new { rm.RoleId, rm.MenuId });
            builder.Entity<RoleMenu>()
              .HasOne(rm => rm.Role)
              .WithMany(r => r.RoleMenus)
              .HasForeignKey(rm => rm.RoleId).HasForeignKey(rm => rm.MenuId);

            //启用Guid主键类型扩展
            builder.HasPostgresExtension("uuid-ossp");

            base.OnModelCreating(builder);
        }
    }
}
