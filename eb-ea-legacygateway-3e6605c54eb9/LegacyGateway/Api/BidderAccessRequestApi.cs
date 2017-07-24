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
    public class BidderAccessRequestApi : IProvisionable<BidderAccessRequest>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidderAccessRequest entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "bidder_access_requests/", method, entity);
            }
            return true;
        }
    }
}
