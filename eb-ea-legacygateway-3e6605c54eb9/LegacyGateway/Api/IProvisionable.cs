using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.Enums;
using LegacyGateway.Enums;
using LegacyGateway.Utilities;

namespace LegacyGateway.Api
{
    public interface IProvisionable<T>
    {
        DatabaseInfo DatabaseInfo { get; set; }
        bool Push(T entity, ChangeType changeType);
    }
}
