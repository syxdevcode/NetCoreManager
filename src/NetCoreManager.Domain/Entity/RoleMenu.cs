using System;

namespace NetCoreManager.Domain.Entity
{
    public class RoleMenu
    {
        public Guid RoleId { get; set; }

        public Role Role { get; set; }

        public Guid MenuId { get; set; }

        public Menu Menu { get; set; }
    }
}