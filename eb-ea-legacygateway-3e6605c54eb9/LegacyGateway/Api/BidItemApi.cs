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
    public class BidItemApi : IProvisionable<BidItem>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidItem entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                SetBidPackageID(entity);
                RestClientHelper.Send(entity.GetType().Name, "section_items", Method.POST, entity);
            }
            else if (changeType == ChangeType.Delete)
            {
                RestClientHelper.Send(entity.GetType().Name, "section_items", Method.DELETE, entity);
            }
            return true;
        }

        public void SetBidPackageID(BidItem entity)
        {
            if (entity.BidSectionID != Guid.Empty)
            {
                string sql =
                    "SELECT " +
                    "   BidPackageID " +
                    $"FROM {DatabaseInfo.AppDatabase}.dbo.daBidSections (NOLOCK) " +
                    $"WHERE BidSectionID = @BidSectionID;";

                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    entity.BidPackageID = DatabaseHelper.QueryFirstOrDefault<Guid>(db, sql, new { BidSectionID = entity.BidSectionID }).Result;
                }
            }
        }
    }
}
