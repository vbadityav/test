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
    public class ContactApi : IProvisionable<Contact>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(Contact entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                SetCompany(entity);
                RestClientHelper.Send(entity.GetType().Name, "contacts", Method.POST, entity);
            }
            else if (changeType == ChangeType.Delete)
            {
                RestClientHelper.Send(entity.GetType().Name, "contacts", Method.DELETE, entity);
            }
            return true;
        }
        private void SetCompany(Contact entity)
        {
            if (entity.CompanyID != Guid.Empty)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@contactID", entity.ContactID);
                    Company company = DatabaseHelper.QueryFirstOrDefault<Company>(db, $"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetCompanyDetailsForContact]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    entity.CompanyID = company.CompanyID;
                    entity.CompanyName = company.CompanyName;
                }
            }
        }
    }
}
