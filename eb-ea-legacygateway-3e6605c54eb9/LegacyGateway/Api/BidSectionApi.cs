using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class BidSectionApi : IProvisionable<BidSection>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidSection entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "sections", method, entity);
            }
            return true;
        }
    }
}
