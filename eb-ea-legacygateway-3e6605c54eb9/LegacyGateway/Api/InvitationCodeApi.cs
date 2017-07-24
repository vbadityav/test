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
    public class InvitationCodeApi : IProvisionable<InvitationCode>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(InvitationCode entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                SetBidPackageID(entity);
                RestClientHelper.Send(entity.GetType().Name, "invitation_codes", Method.POST, entity);
            }
            else if (changeType == ChangeType.Delete)
            {
                RestClientHelper.Send(entity.GetType().Name, "invitation_codes", Method.DELETE, entity);
            }
            return true;
        }

        public void SetBidPackageID(InvitationCode entity)
        {
            if (entity.InvitationID != Guid.Empty)
            {
                string sql = 
                    "SELECT " +
                    "   BidPackageID " +
                    $"FROM {DatabaseInfo.AppDatabase}.dbo.daInvitations (NOLOCK) " +
                    $"WHERE InvitationID = @InvitationID;";

                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    entity.BidPackageID = DatabaseHelper.QueryFirstOrDefault<Guid>(db, sql, new { InvitationID = entity.InvitationID }).Result;
                }
            }
        }
    }
}
