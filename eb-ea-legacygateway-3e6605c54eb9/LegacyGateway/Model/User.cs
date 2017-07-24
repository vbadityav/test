using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daUsers")]
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public Guid UserID { get; set; }
        [JsonProperty(PropertyName = "first_name")]
        public string givenName { get; set; }
        [JsonProperty(PropertyName = "last_name")]
        public string sn { get; set; }
        [JsonProperty(PropertyName = "company")]
        public string CompanyName { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime ?CreateDate { get; set; }
        [JsonProperty(PropertyName = "user_name")]
        public string cn { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "office_street")]
        public string OfficeStreet { get; set; }
        [JsonProperty(PropertyName = "office_po_box")]
        public string OfficePOBox { get; set; }
        [JsonProperty(PropertyName = "office_city")]
        public string OfficeCity { get; set; }
        [JsonProperty(PropertyName = "office_state")]
        public string OfficeState { get; set; }
        [JsonProperty(PropertyName = "office_zip")]
        public string OfficeZip { get; set; }
        [JsonProperty(PropertyName = "office_country")]
        public string OfficeCountry { get; set; }
        [JsonProperty(PropertyName = "office_phone")]
        public string OfficePhone { get; set; }
        [JsonProperty(PropertyName = "office_fax")]
        public string OfficeFax { get; set; }
        [JsonProperty(PropertyName = "office_mobile_phone")]
        public string OfficeMobilePhone { get; set; }
        [JsonProperty(PropertyName = "office_pager")]
        public string OfficePager { get; set; }
        [JsonProperty(PropertyName = "creator_user_id")]
        public Guid? Creator { get; set; }
        [JsonProperty(PropertyName = "department")]
        public string Department { get; set; }
        [JsonProperty(PropertyName = "job_title")]
        public string JobTitle { get; set; }
        [JsonProperty(PropertyName = "business_type")]
        public string BusinessType { get; set; }
        [JsonProperty(PropertyName = "last_login_date")]
        public DateTime? LastLogon { get; set; }
        [JsonProperty(PropertyName = "locked")]
        public bool? IsLocked { get; set; }
        [JsonProperty(PropertyName = "password_change_required")]
        public bool? IsPWChangeRequired { get; set; }
        [JsonProperty(PropertyName = "date_password_changed")]
        public DateTime? PasswordChangeDate { get; set; }
        [JsonProperty(PropertyName = "notification_time_zone")]
        public string NotificationTimeZones { get; set; }
        [JsonProperty(PropertyName = "failed_login_attempt_count")]
        public int? FailedLoginAttempts { get; set; }
        [JsonProperty(PropertyName = "date_locked_out")]
        public DateTime? LockedOutSince { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string EncryptedPassword { get; set; }

        // Type = 12
        [JsonProperty(PropertyName = "security_questions")]
        public List<SecurityQuestions> SecurityQuestions { get; set; }

        public bool ShouldSerializeSecurityQuestions()
        {
            if (SecurityQuestions == null) return false;
            return SecurityQuestions.Any();
        }
    }
}
