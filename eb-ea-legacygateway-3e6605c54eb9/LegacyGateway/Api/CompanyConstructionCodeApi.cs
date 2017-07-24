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
    class CompanyConstructionCodeApi : IProvisionable<CompanyConstructionCode>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(CompanyConstructionCode entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "company_construction_codes", method, entity);
            }
            return true;
        }
    }
}
