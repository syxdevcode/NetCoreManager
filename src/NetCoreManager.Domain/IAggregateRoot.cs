using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
