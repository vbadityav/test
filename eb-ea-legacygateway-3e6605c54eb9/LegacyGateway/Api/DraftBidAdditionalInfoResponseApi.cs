using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using System;
using System.Data;
using System.Data.SqlClient;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class DraftBidAdditionalInfoResponseApi : IProvisionable<DraftBidAdditionalInfoResponse>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(DraftBidAdditionalInfoResponse entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                SetBidID(entity);
                RestClientHelper.Send(entity.GetType().Name, "bid_additional_info_responses", Method.POST, entity);
            }
            else if(changeType == ChangeType.Delete)
            {
                RestClientHelper.Send(entity.GetType().Name, "bid_additional_info_responses", Method.DELETE, entity);
            }
            return true;
        }

        // Legacy uses BidID instead of DraftBidID, new stack needs the DraftBidID
        public void SetBidID(DraftBidAdditionalInfoResponse entity)
        {
            if (entity.BidID != Guid.Empty)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@BidID", entity.BidID);

                    string sql =
                        $"SELECT DraftBidID FROM {DatabaseInfo.AppDatabase}.dbo.daDraftBids (NOLOCK) WHERE BidID = @BidID;";

                    entity.BidID = DatabaseHelper.QueryFirstOrDefault<Guid>(db, sql, sp_params).Result;
                }
            }
        }
    }
}
