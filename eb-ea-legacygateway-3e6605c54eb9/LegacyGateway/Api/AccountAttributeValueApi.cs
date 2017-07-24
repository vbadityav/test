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
    public class AccountAttributeValueApi : IProvisionable<AccountAttributeValue>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(AccountAttributeValue entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            string attributeName = GetAccountAttributeName(entity);
            if (attributeName.ToLower() == "ebmaxfailedloginattempts")
            {
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
                {
                    RestClientHelper.Send(entity.GetType().Name, "accounts/set_max_failed_login_attempts/", Method.POST, new {
                        id = entity.AccountID,
                        value = entity.i_Value
                    });
                }
                else if (changeType == ChangeType.Delete)
                {
                    RestClientHelper.Send(entity.GetType().Name, "accounts/set_max_failed_login_attempts/", Method.POST, new
                    {
                        id = entity.AccountID,
                        value = 0
                    });
                }
            }
            else if (attributeName.ToLower() == "enablebidresubmission")
            {
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
                {
                    Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                    RestClientHelper.Send(entity.GetType().Name, "bid_package/enable_bid_resubmission", method, new
                    {
                        id = entity.AccountID,
                        value = (entity.i_Value == 1)
                    });
                }
            }
            else if (attributeName.ToLower() == "bidpastduedatemessage")
            {
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
                {
                    Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                    RestClientHelper.Send(entity.GetType().Name, "bid_package/past_due_message", method, new
                    {
                        id = entity.AccountID,
                        value = entity.vc_Value
                    });
                }
            }
            else if (attributeName.ToLower() == "ebfailedloginlockoutduration")
            {
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
                {
                    Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                    RestClientHelper.Send(entity.GetType().Name, "accounts/set_lockout_duration/", method, new
                    {
                        id = entity.AccountID,
                        value = entity.i_Value
                    });
                }
            }

            return true;
        }

        private string GetAccountAttributeName(AccountAttributeValue entity)
        {
            if (entity.AttributeID != Guid.Empty)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@AttributeID", entity.AttributeID);
                    AccountAttribute attribute = DatabaseHelper.QueryFirstOrDefault<AccountAttribute>(db, $"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetAccountAttributeDetails]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    return attribute.Name;
                }
            }
            return string.Empty;
        }
    }
}
