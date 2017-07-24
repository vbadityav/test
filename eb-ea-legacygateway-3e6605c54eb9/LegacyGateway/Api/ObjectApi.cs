using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class ObjectApi : IProvisionable<ebObject>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(ebObject entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                if(entity.IsDeleted && entity.ObjectType == 2)
                {
                    entity.Files = GetFiles(entity.ObjectID);
                }
                Method method = entity.IsDeleted ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "objects", method, entity);
            }
            return true;
        }

        private List<File> GetFiles(Guid objectID)
        {
            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                var sp_params = new DynamicParameters();
                sp_params.Add("@objectID", objectID);
                return db.Query<File>($"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetFileVersions]", sp_params, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
