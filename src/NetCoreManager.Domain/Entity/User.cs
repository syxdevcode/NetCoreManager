using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.Domain.Entity
{
    public class User: IAggregateRoot
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AddTime { get; set; }

        public bool IsDelete { get; set; }
    }
}
