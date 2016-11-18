using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.Domain.Entity
{
    public class Department: IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 父级部门Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 用户集合
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}
