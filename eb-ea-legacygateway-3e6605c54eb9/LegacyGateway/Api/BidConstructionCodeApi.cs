using System;
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
    public class BidPackageConstructionCodeApi : IProvisionable<BidPackageConstructionCode>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidPackageConstructionCode entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                SetConstructionCode(entity);

                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "bid_package_construction_codes", method, entity);
            }
            return true;
        }

        private void SetConstructionCode(BidPackageConstructionCode entity)
        {
            if (entity.ConstructionCodeID != Guid.Empty)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@constructionCodeID", entity.ConstructionCodeID);
                    ConstructCode code = DatabaseHelper.QueryFirstOrDefault<ConstructCode>(db, $"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetConstructionCodeDetails]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    entity.Code = code;
                }
            }
        }
    }
}
