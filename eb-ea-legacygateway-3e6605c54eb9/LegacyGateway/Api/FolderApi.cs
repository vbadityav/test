using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class FolderApi : IProvisionable<Folder>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(Folder entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "folders", method, entity);
            }
            return true;
        }
    }
}
