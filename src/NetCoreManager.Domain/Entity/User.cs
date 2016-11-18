using System;
using System.Collections.Generic;

namespace NetCoreManager.Domain.Entity
{
    public class User : IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 所属部门实体
        /// </summary>
        public virtual Department Department { get; set; }

        /// <summary>
        /// 角色集合
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}