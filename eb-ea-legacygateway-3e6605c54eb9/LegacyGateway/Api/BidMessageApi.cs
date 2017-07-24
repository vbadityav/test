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
    public class BidderMessageApi : IProvisionable<BidderMessage>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidderMessage entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            if(changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                if (entity.InvitationID == Guid.Empty)
                {
                    entity.InvitationID = null;
                }

                if (entity.isQuestion)
                {
                    SetQuestion(entity);
                }
                else if (entity.SenderUserID.HasValue && entity.SenderUserID != Guid.Empty) // Get user information fromn SenderUserID for responder
                {
                    SetResponse(entity);
                }

                Method method = entity.IsDeleted ? Method.DELETE : Method.POST;
                RestClientHelper.Send(entity.GetType().Name, entity.isQuestion ? "questions" : "responses", method, entity);
            }
            return true;
        }

        private void SetResponse(BidderMessage entity)
        {
            entity.Response = entity.Body;
            entity.ResponseSent = entity.DateSent;
            entity.ResponseID = entity.MessageID;

            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                var sp_params = new DynamicParameters();
                sp_params.Add("@userID", entity.SenderUserID);
                Contact contact = DatabaseHelper.QueryFirstOrDefault<Contact>(db, $"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetUserDetails]", sp_params, commandType: CommandType.StoredProcedure).Result;
                entity.Contact = contact;

                sp_params = new DynamicParameters();
                sp_params.Add("@messageID", entity.MessageID);
                entity.QuestionID = DatabaseHelper.QueryFirstOrDefault<Guid>(db, $"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetBidPackageQuestionIDForResponseID]", sp_params, commandType: CommandType.StoredProcedure).Result;
            }
        }

        private void SetQuestion(BidderMessage entity)
        {
            entity.Question = entity.Body;
            entity.QuestionSent = entity.DateSent;
            entity.QuestionID = entity.MessageID;

            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                var sp_params = new DynamicParameters();
                if (entity.InvitationID.HasValue) // Get contact information from Invitation for sender
                {
                    sp_params.Add("@invitationID", entity.InvitationID);
                    Contact contact = DatabaseHelper.QueryFirstOrDefault<Contact>(db, $"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetContactDetailsFromInvitation]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    entity.Contact = contact;
                }
                else // Get user information from SenderUserID for sender
                {
                    sp_params = new DynamicParameters();
                    sp_params.Add("@userID", entity.SenderUserID);
                    Contact contact = DatabaseHelper.QueryFirstOrDefault<Contact>(db, $"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetUserDetails]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    entity.Contact = contact;
                }
            }
        }
    }
}
