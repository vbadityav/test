using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class InvitationApi : IProvisionable<Invitation>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(Invitation entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            if (changeType == ChangeType.Insert ||changeType == ChangeType.Update)
            {
                RestClientHelper.Send(entity.GetType().Name, "invitations", Method.POST, entity, success => {
                    SetContact(entity);
                });
            }
            else if (changeType == ChangeType.Delete)
            {
                RestClientHelper.Send(entity.GetType().Name, "invitations", Method.DELETE, entity);
            }
            return true;
        }

        private void SetContact(Invitation entity)
        {
            if (entity.ContactID != Guid.Empty)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@contactID", entity.ContactID);
                    Contact contact = DatabaseHelper.QueryFirstOrDefault<Contact>(db, $"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetContactDetails]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    RestClientHelper.Send(entity.GetType().Name + " - Contact", $"invitations/{entity.InvitationID}/contact", Method.POST, contact);
                }
            }
        }
    }
}
