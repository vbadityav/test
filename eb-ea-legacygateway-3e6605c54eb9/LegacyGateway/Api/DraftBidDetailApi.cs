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
    public class DraftBidDetailApi : IProvisionable<DraftBidDetail>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(DraftBidDetail entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                SetDraftBidID(entity);
                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, "bid_details", method, entity);
            }
            return true;
        }

        private void SetDraftBidID(DraftBidDetail entity)
        {
            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                entity.BidID = DatabaseHelper.QueryFirstOrDefault<Guid>(db, $"select db.DraftBidID from {DatabaseInfo.AppDatabase}.dbo.daDraftBidDetails dbd (nolock) inner join {DatabaseInfo.AppDatabase}.dbo.daBids b (nolock) on b.BidID = dbd.BidID inner join {DatabaseInfo.AppDatabase}.dbo.daDraftBids db (nolock) on db.InvitationID = b.InvitationID WHERE dbd.BidID = '{entity.BidID}'").Result;
            }
        }
    }
}
