using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class CustomFieldDependenyApi : IProvisionable<CustomFieldDependency>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(CustomFieldDependency entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
            RestClientHelper.Send(entity.GetType().Name, "custom_field_dependencies", method, entity);
            return true;
        }
    }
}
