using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    class BidPackageApi : IProvisionable<BidPackage>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(BidPackage entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");

            if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                SetOwnerAndProject(entity);
                
                RestClientHelper.Send(entity.GetType().Name, "bid_package", Method.POST, entity);
            }
            return true;
        }

        private void SetOwnerAndProject(BidPackage entity)
        {

            if (entity.BidStartDate.HasValue)
            {
                entity.BidStartDate = LegacyBL.Globals.ToGMT(LegacyBL.Globals.GetTimeZoneName(entity.BidTimeZone), entity.BidStartDate.Value);
            }
            if (entity.BidResponseDate.HasValue)
            {
                entity.BidResponseDate = LegacyBL.Globals.ToGMT(LegacyBL.Globals.GetTimeZoneName(entity.BidTimeZone), entity.BidResponseDate.Value);
            }
            if (entity.PreBidMeetingDate.HasValue)
            {
                entity.PreBidMeetingDate = LegacyBL.Globals.ToGMT(LegacyBL.Globals.GetTimeZoneName(entity.BidTimeZone), entity.PreBidMeetingDate.Value);
            }
            if (entity.HardBidCutoffDate.HasValue)
            {
                entity.HardBidCutoffDate = LegacyBL.Globals.ToGMT(LegacyBL.Globals.GetTimeZoneName(entity.BidTimeZone), entity.HardBidCutoffDate.Value);
            }

            var sp_params = new DynamicParameters();
            sp_params.Add("@bidpackageid", entity.BidPackageID);
            sp_params.Add("@folderID", entity.BidDocsFolderID);
            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                using (var multi = db.QueryMultiple($"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetBidPackageDetails]", sp_params, commandType: CommandType.StoredProcedure))
                {
                    var packageInfo = multi.ReadFirst();
                    entity.BidDocsFolderName = (string)packageInfo.folder_name;
                    entity.ResponseFolderName = (string)packageInfo.response_folder_name;

                    entity.BidItemCustomField1 = packageInfo.custom_field_1;
                    entity.BidItemCustomField2 = packageInfo.custom_field_2;
                    entity.BidItemCustomField3 = packageInfo.custom_field_3;

                    entity.Owner = new BidPackageOwner()
                    {
                        user_id = (Guid)packageInfo.user_id,
                        first_name = (string)packageInfo.first_name,
                        last_name = (string)packageInfo.last_name,
                        phone = (string)packageInfo.phone,
                        email = (string)packageInfo.email
                    };
                    entity.Project = new Portal()
                    {
                        PortalID = (Guid)packageInfo.portal_id,
                        Name = (string)packageInfo.portal_name,
                        FileStorePath = (string)packageInfo.file_store_path,
                        Account = new BidPackageAccountData()
                        {
                            AccountID = (Guid)packageInfo.account_id,
                            Name = (string)packageInfo.account_name,
                            PortalType = new PortalType()
                            {
                                PortalTypeID = (int)packageInfo.portal_type_id,
                                DBName_App = (string)packageInfo.portal_type_app_name
                            }
                        }
                    };

                    entity.Documents = multi.Read<Document>().ToList();
                    entity.FilePaths = multi.Read<FilePath>().ToList();
                    entity.BidSubmissionCustomFields = multi.Read<CustomField>().ToList();

                    Preference publicRegistrationFields = multi.ReadFirstOrDefault<Preference>();
                    if (publicRegistrationFields != null)
                    {
                        PublicBiddingFieldWrapper bidderFields = new PublicBiddingFieldWrapper(publicRegistrationFields.Value);
                        entity.CompanyFields = bidderFields.company_fields;
                        entity.CompanyCustomFields = bidderFields.company_custom_fields;
                        entity.ContactFields = bidderFields.contact_fields;
                        entity.ContactCustomFields = bidderFields.contact_custom_fields;
                    }

                    entity.EnableBidResubmission = multi.ReadFirstOrDefault<bool>();

                    entity.PastDueMessage = multi.ReadFirstOrDefault<string>();
                }
            }
        }
    }
}
