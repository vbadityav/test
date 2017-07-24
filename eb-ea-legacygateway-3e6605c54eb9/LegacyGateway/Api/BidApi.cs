using System.Data;
using System.Data.SqlClient;
using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class BidApi : IProvisionable<Bid>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(Bid entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
            RestClientHelper.Send(entity.GetType().Name, "bids", method, entity);
            return true;
        }
    }
}
