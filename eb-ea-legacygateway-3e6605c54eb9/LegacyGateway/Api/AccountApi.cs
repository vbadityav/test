using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class AccountApi : IProvisionable<Account>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(Account entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                SetAccountProperties(entity);
                RestClientHelper.Send(entity.GetType().Name, "accounts", Method.POST, entity);
            }
            else if (changeType == ChangeType.Delete)
            {
                RestClientHelper.Send(entity.GetType().Name, "accounts", Method.DELETE, entity);
            }
            return true;
        }

        private void SetAccountProperties(Account entity)
        {
            if (entity.AccountID != Guid.Empty)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@accountID", entity.AccountID);
                    Account account = db.Query<Account, PortalType, Account>($"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetAccountDetails]", (a, pt) =>
                    {
                        a.PortalType = pt;
                        return a;
                    }, sp_params, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    entity.ebMaxFailedLoginAttempts = account.ebMaxFailedLoginAttempts;
                    entity.ebFailedLoginLockoutDuration = account.ebFailedLoginLockoutDuration;
                    entity.PortalType = account.PortalType;
                }
            }
        }
    }
}
