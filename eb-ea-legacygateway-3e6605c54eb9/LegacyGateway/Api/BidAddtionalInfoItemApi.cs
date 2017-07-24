using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class BidAdditionalInfoItemApi : IProvisionable<BidAdditionalInfoItem>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidAdditionalInfoItem entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "additional_information", method, entity);
            }
            return true;
        }
    }
}
