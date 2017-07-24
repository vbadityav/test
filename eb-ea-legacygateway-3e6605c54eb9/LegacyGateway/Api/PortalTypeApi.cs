using LegacyGateway.Model;
using LegacyGateway.Utilities;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class PortalTypeApi : IProvisionable<PortalType>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(PortalType entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            return true;
        }
    }
}
