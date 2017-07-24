using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyGateway.Api;
using Newtonsoft.Json;
using LegacyGateway.DTO;

namespace LegacyGateway.Model
{
    [Table("daBidPackages")]
    public class BidPackage
    {
        [JsonProperty(PropertyName = "id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; internal set; }
        [JsonProperty(PropertyName = "project")]
        public Portal Project { get; set; }
        [JsonProperty(PropertyName = "due_date")]
        public DateTime? BidResponseDate { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string NameEnum { get { return Status == 0 ? "Closed" : "Open"; } }
        [JsonProperty(PropertyName = "time_zone")]
        public int BidTimeZone { get; set; }
        [JsonProperty(PropertyName = "start_date")]
        public DateTime? BidStartDate { get; set; }
        [JsonProperty(PropertyName = "instructions_file_id")]
        public Guid? BidInstructionsFileID { get; set; }
        [JsonProperty(PropertyName = "allow_bids_after_response_date")]
        public bool AllowBidsAfterResponseDate { get; set; }
        [JsonProperty(PropertyName = "allow_electronic_bids")]
        public bool AllowElectronicBids { get; set; }
        [JsonProperty(PropertyName = "square_feet")]
        public decimal? BidSqFt { get; set; }

        [JsonProperty(PropertyName = "created_by_id")]
        public Guid CreatedByID { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "enable_plan_room")]
        public bool? EnablePlanRoom { get; set; }
        [JsonProperty(PropertyName = "enable_gpo_integration")]
        public bool? EnableGPOIntegration { get; set; }
        [JsonProperty(PropertyName = "is_notice_of_opportunity")]
        public bool? IsNoticeOfOpportunity { get; set; }

        [JsonProperty(PropertyName = "last_modified_by_id")]
        public Guid LastModifiedByID { get; set; }
        [JsonProperty(PropertyName = "date_last_modified")]
        public DateTime LastModifiedDate { get; set; }
        [JsonProperty(PropertyName = "is_pre_bid_meeting_required")]
        public bool IsPreBidMeetingRequired { get; set; }
        [JsonProperty(PropertyName = "pre_bid_meeting_date")]
        public DateTime? PreBidMeetingDate { get; set; }
        [JsonProperty(PropertyName = "pre_bid_meeting_location")]
        public string PreBidMeetingLocation { get; set; }
        [JsonProperty(PropertyName = "hard_cutoff_date")]
        public DateTime? HardBidCutoffDate { get; set; }
        [JsonProperty(PropertyName = "is_documentation_required")]
        public bool IsDocumentationRequired { get; set; }
        [JsonProperty(PropertyName = "is_public")]
        public bool IsPublic { get; set; }
        [JsonProperty(PropertyName = "allow_bid_resubmission")]
        public bool? EnableBidResubmission { get; set; }
        [JsonProperty(PropertyName = "past_due_message")]
        public string PastDueMessage { get; set; }
        [JsonProperty(PropertyName = "owner")]
        public BidPackageOwner Owner { get; set; }
        [JsonProperty(PropertyName = "document_folder")]
        public Folder BidDocsFolder
        {
            get
            {
                if (BidDocsFolderID != Guid.Empty)
                {
                    return new Folder()
                    {
                        FolderID = BidDocsFolderID,
                        FolderName = BidDocsFolderName
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        [JsonProperty(PropertyName = "response_folder")]
        public Folder ResponseFolder
        {
            get
            {
                if (ResponseFolderID != Guid.Empty)
                {
                    return new Folder()
                    {
                        FolderID = ResponseFolderID,
                        FolderName = ResponseFolderName
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        [JsonProperty(PropertyName = "documents")]
        public List<Document> Documents { get; set; }
        [JsonProperty(PropertyName = "file_paths")]
        public List<FilePath> FilePaths { get; set; }
        [JsonProperty(PropertyName = "section_item_custom_fields")]
        public List<BidPackageCustomField> CustomFields
        {
            get
            {
                List<BidPackageCustomField> list = new List<BidPackageCustomField>();
                if (!String.IsNullOrWhiteSpace(BidItemCustomField1))
                {
                    list.Add(new BidPackageCustomField(BidItemCustomField1, 1));
                }
                if (!String.IsNullOrWhiteSpace(BidItemCustomField2))
                {
                    list.Add(new BidPackageCustomField(BidItemCustomField2, 2));
                }
                if (!String.IsNullOrWhiteSpace(BidItemCustomField3))
                {
                    list.Add(new BidPackageCustomField(BidItemCustomField3, 3));
                }
                if(list.Count > 0)
                {
                    return list;
                }
                return null;
            }
        }
        [JsonProperty(PropertyName = "bid_submission_custom_fields")]
        public List<CustomField> BidSubmissionCustomFields { get; set; }
        [JsonIgnore]
        public Guid OwnerID { get; set; }
        [JsonIgnore]
        public string BidItemCustomField1 { get; set; }
        [JsonIgnore]
        public string BidItemCustomField2 { get; set; }
        [JsonIgnore]
        public string BidItemCustomField3 { get; set; }
        [JsonIgnore]
        public Guid? BidDocsFolderID { get; set; }
        [JsonIgnore]
        public string BidDocsFolderName { get; set; }
        [JsonIgnore]
        public Guid? ResponseFolderID { get; set; }
        [JsonIgnore]
        public string ResponseFolderName { get; set; }
        [JsonIgnore]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "linked_bid_package_id")]
        public Guid? LinkedBidPackageID { get; set; }


        [JsonProperty(PropertyName = "contact_fields")]
        public IEnumerable<ebPublicBiddingField> ContactFields { get; set; }
        [JsonProperty(PropertyName = "contact_custom_fields")]
        public IEnumerable<ebPublicBiddingField> ContactCustomFields { get; set; }
        [JsonProperty(PropertyName = "company_fields")]
        public IEnumerable<ebPublicBiddingField> CompanyFields { get; set; }
        [JsonProperty(PropertyName = "company_custom_fields")]
        public IEnumerable<ebPublicBiddingField>CompanyCustomFields { get; set; }

        public bool ShouldSerializeLinkedBidPackageID()
        {
            return LinkedBidPackageID != Guid.Empty;
        }
    }

    public class BidPackageOwner
    {
        [JsonProperty(PropertyName = "id")]
        public Guid user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
