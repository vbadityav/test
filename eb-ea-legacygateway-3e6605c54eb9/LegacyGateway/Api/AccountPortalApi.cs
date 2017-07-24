using LegacyGateway.Model;
using LegacyGateway.Utilities;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class AccountPortalApi : IProvisionable<AccountPortal>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(AccountPortal entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            return true;
        }
    }
}
