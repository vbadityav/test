using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LegacyGateway.Utilities
{
    public class DatabaseInfo
    {
        public string ConnectionString { get; }
        public string AppDatabase { get; set; }
        public string CoreDatabase { get; set; }
        public bool IsAppDefined { get { return !string.IsNullOrWhiteSpace(AppDatabase); } }

        public DatabaseInfo()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        }

        public DatabaseInfo(string appName, string coreName): this()
        {
            AppDatabase = appName;
            CoreDatabase = coreName;
        }

        public string GetAppConnectionString()
        {
            return $"{ConnectionString}Database={AppDatabase}";
        }

        public string GetCoreConnectionString()
        {
            return $"{ConnectionString}Database={CoreDatabase}";
        }
    }
}
