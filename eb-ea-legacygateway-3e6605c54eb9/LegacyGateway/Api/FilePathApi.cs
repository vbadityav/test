using System;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class FilePathApi : IProvisionable<FilePath>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(FilePath entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (entity.PathTypeID != 1 && entity.PathTypeID != 4)
            {
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
                {
                    Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                    RestClientHelper.Send(entity.GetType().Name, "file_paths", method, entity);
                }
            }
            return true;
        }
    }
}
