using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daAccounts")]
    public class Account
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AccountID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonIgnore]
        public string PortalTypeID { get; set; }
        [JsonProperty(PropertyName = "portal_type")]
        public PortalType PortalType { get; set; }
        [JsonProperty(PropertyName = "is_active")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "url_safe_name")]
        public string UrlSafeName { get; set; }
        [JsonProperty(PropertyName = "account_logo_height")]
        public int AccountLogoHeight { get; set; }
        [JsonProperty(PropertyName = "permission_last_updated")]
        public DateTime PermissionLastUpdated { get; set; }
        [JsonProperty(PropertyName = "max_failed_login_attempts")]
        public int ebMaxFailedLoginAttempts { get; set; }
        [JsonProperty(PropertyName = "lockout_duration")]
        public int ebFailedLoginLockoutDuration { get; set; }

    }

    public class BidPackageAccountData
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AccountID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonIgnore]
        public string PortalTypeID { get; set; }
        [JsonProperty(PropertyName = "portal_type")]
        public PortalType PortalType { get; set; }

    }
}
