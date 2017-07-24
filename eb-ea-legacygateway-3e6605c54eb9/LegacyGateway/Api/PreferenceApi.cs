using System;
using System.Collections.Generic;
using System.Linq;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using LegacyGateway.DTO;
using System.IO;
using System.Xml.Serialization;

namespace LegacyGateway.Api
{
    public class PreferenceApi : IProvisionable<Preference>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(Preference entity, ChangeType changeType)
        {
            if (entity.Type == 12) // Security Questions
            {
                _log.Info($"{entity.GetType().Name} - {changeType}");
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
                {
                    List<SecurityQuestions> questions = SetSecurityQuestions(entity);
                    if (questions != null)
                    {
                        RestClientHelper.Send(entity.GetType().Name, $"users/{entity.UserID}/security", Method.POST, new
                        {
                            security_questions = questions
                        });
                    }
                }
            }
            else if (entity.Type == 18) // BidDocPermissionRole
            {
                _log.Info($"{entity.GetType().Name} - {changeType}");
                // Send Array of Documents with FilePaths
                BidDocumentsWrapper data = GetBidDocuments(entity, changeType);
                RestClientHelper.Send(entity.GetType().Name, "bid_package/documents", Method.POST, data);
            }
            else if (entity.Type == 67) //PublicBiddingFields
            {
                _log.Info($"{entity.GetType().Name} - {changeType}");
                ebPublicBiddingField[] fields = GetPublicBidderFields(entity);
                var group = from f in fields 
                            group f by f.GroupName into g
                            select g;

                RestClientHelper.Send(entity.GetType().Name, "bid_package/public_bidder_settings", Method.POST, new {
                    account_id = entity.AccountID,
                    company_fields = group.FirstOrDefault(g => g.Key == "Company Fields"),
                    company_custom_fields = group.FirstOrDefault(g => g.Key == "Company Custom Fields"),
                    contact_fields = group.FirstOrDefault(g => g.Key == "Contact Fields"),
                    contact_custom_fields = group.FirstOrDefault(g => g.Key == "Contact Custom Fields")
                });
            }
            return true;
        }

        private BidDocumentsWrapper GetBidDocuments(Preference entity, ChangeType changeType)
        {
            BidDocumentsWrapper data = new BidDocumentsWrapper()
            {
                BidDocuments = new List<BidDocuments>()
            };
            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                var sp_params = new DynamicParameters();
                sp_params.Add("@accountID", entity.AccountID.Value);
                string appname = db.QueryFirstOrDefault<string>($"{DatabaseInfo.CoreDatabase}.dbo.[Gateway_GetAccountAppDatabase]", sp_params, commandType: CommandType.StoredProcedure);

                List<BidDocument> documents = new List<BidDocument>();
                List<BidFilePath> filePaths = new List<BidFilePath>();

                string sp = "[Gateway_GetBidDocuments]";
                if (changeType != ChangeType.Delete)
                {
                    sp_params.Add("@bidDocumentRoleID", new Guid(entity.Value));
                    sp = "[Gateway_GetBidDocumentsByRole]";
                }
                using (var multi = db.QueryMultiple($"{appname}.dbo.{sp}", sp_params, commandType: CommandType.StoredProcedure))
                {
                    documents.AddRange(multi.Read<BidDocument>().ToList());
                    filePaths.AddRange(multi.Read<BidFilePath>().ToList());
                }
                List<Guid> bidPackageIDs = documents.Select(d => d.BidPackageID).Distinct().ToList();
                foreach (Guid bidPackageID in bidPackageIDs)
                {
                    data.BidDocuments.Add(new BidDocuments()
                    {
                        BidPackageID = bidPackageID,
                        Documents = documents.Where(d => d.BidPackageID == bidPackageID).Select(d => d as Document),
                        FilePaths = filePaths.Where(fp => fp.BidPackageID == bidPackageID).Select(fp => fp as FilePath)
                    });
                }
            }
            return data;
        }

        private List<SecurityQuestions> SetSecurityQuestions(Preference entity)
        {
            if (entity.UserID != Guid.Empty)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ebUserResponses));
                List<SecurityQuestions> securityQuestions = new List<SecurityQuestions>();
                using (StringReader sr = new StringReader(entity.Value))
                {
                    ebUserResponses v = (ebUserResponses)serializer.Deserialize(sr);
                    securityQuestions.Add(new SecurityQuestions() { Question = v.Question1, Answer = v.Answer1 });
                    securityQuestions.Add(new SecurityQuestions() { Question = v.Question2, Answer = v.Answer2 });
                    securityQuestions.Add(new SecurityQuestions() { Question = v.Question3, Answer = v.Answer3 });
                }
                return securityQuestions;
            }
            return null;
        }
        private ebPublicBiddingField[] GetPublicBidderFields(Preference entity)
        {
            if (entity.UserID != Guid.Empty)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ebPublicBiddingField[]));
                ebPublicBiddingField[] fields;
                using (StringReader sr = new StringReader(entity.Value))
                {
                    fields = (ebPublicBiddingField[])serializer.Deserialize(sr);
                }
                return fields;
            }
            return null;
        }
    }
}
