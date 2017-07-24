using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;
using LegacyGateway.Model;
using LegacyGateway.Api;
using System.Configuration;
using LegacyGateway.Utilities;

namespace LegacyGateway
{
    class LegacyGatewayService
    {
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();
        private List<ITableWatcher> watchers = new List<ITableWatcher>();

        public bool Start()
        {
            string coreDB = ConfigurationManager.AppSettings["CoreDatabase"];
            string appDBs = ConfigurationManager.AppSettings["AppDatabases"];

            DatabaseHelper.Initalize();


            Console.WriteLine("App Settings:");
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                Console.WriteLine($"{key}: {ConfigurationManager.AppSettings[key]}");
            }

            //Setup Core watchers
            if (!string.IsNullOrWhiteSpace(coreDB))
            {
                try
                {
                    Console.WriteLine(Environment.NewLine + "Database: ebCore");
                    DatabaseInfo dbInfo = new DatabaseInfo(string.Empty, coreDB);
                    watchers.Add(new TableWatcher<Account, AccountApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<AccountAttributeValue, AccountAttributeValueApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<Portal, PortalApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<AccountPortal, AccountPortalApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<PortalType, PortalTypeApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<User, UserApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<AccountUser, AccountUserApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<Contact, ContactApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<Company, CompanyApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<ConstructCode, ConstructionCodeApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<NetworkAnnouncement, NetworkAnnouncementApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<Preference, PreferenceApi>(dbInfo, true));
                    watchers.Add(new TableWatcher<CompanyConstructionCode, CompanyConstructionCodeApi>(dbInfo, true));
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }

            //Setup App watchers
            if (!string.IsNullOrWhiteSpace(appDBs))
            {
                foreach (string db in appDBs.Split(','))
                {
                    try
                    {
                        Console.WriteLine(Environment.NewLine + "Database: " + db);
                        // Order matters! Place in order you need to process them
                        DatabaseInfo dbInfo = new DatabaseInfo(db, coreDB);
                        watchers.Add(new TableWatcher<BidPackage, BidPackageApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<File, FileApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<Invitation, InvitationApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidPackageConstructionCode, BidPackageConstructionCodeApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidSection, BidSectionApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidItem, BidItemApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidAddenda, BidAddendaApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidAdditionalInfoItem, BidAdditionalInfoItemApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidAdditionalInfoResponse, BidAdditionalInfoResponseApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<Bid, BidApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidDetail, BidDetailApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<DraftBid, DraftBidApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<DraftBidDetail, DraftBidDetailApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<DraftBidAdditionalInfoResponse, DraftBidAdditionalInfoResponseApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidderMessage, BidderMessageApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<ebObject, ObjectApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<Folder, FolderApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<FilePath, FilePathApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<InvitationCode, InvitationCodeApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<CustomField, CustomFieldApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<CustomFieldValue, CustomFieldValueApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<CustomFieldDependency, CustomFieldDependenyApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidderAccessRequest, BidderAccessRequestApi>(dbInfo, true));
                        watchers.Add(new TableWatcher<BidderReopenRequest, BidderReopenRequestApi>(dbInfo, true));
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                    }
                }
            }

            return true;
        }

        public bool Stop()
        {
            foreach(ITableWatcher watcher in watchers)
            {
                try
                {
                    watcher.Stop();
                }
                catch(Exception ex)
                {
                    _log.Error(ex.Message);
                }
            }
            return true;
        }

        public bool Pause()
        {
            return true;
        }

        public bool Continue()
        {
            return true;
        }

        public void CustomCommand(int commandNumber)
        {

        }
    }
}
