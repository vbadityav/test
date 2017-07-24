using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace LegacyGateway.Utilities
{
    public class DatabaseHelper
    {
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public static int RetryCount = 3;
        public static int RetryInterval = 5000;

        public static void Initalize()
        {
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RetryCount"]))
            {
                int.TryParse(ConfigurationManager.AppSettings["RetryCount"], out RetryCount);
            }

            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RetryInterval"]))
            {
                int.TryParse(ConfigurationManager.AppSettings["RetryInterval"], out RetryInterval);
            }
        }

        public static async Task<T> QueryFirstOrDefault<T>(IDbConnection db, string sql, object param = null, CommandType? commandType = null)
        {
            T result = default(T);
            for (int retryCount = 0; retryCount < RetryCount - 1; retryCount++)
            {
                result = await db.QueryFirstOrDefaultAsync<T>(sql, param, commandType: commandType);
                if (result != null)
                {
                    return result;
                }
                else if ((retryCount + 1) < RetryCount)
                {
                    _log.Warn($"Retry [{retryCount}]:" + sql);
                    System.Threading.Thread.Sleep(RetryInterval);
                }
            }
            return result;
        }

        public static async Task<IEnumerable<T>> Query<T>(IDbConnection db, string sql, object param = null, CommandType? commandType = null)
        {
            IEnumerable<T> result = null;
            for (int retryCount = 0; retryCount < RetryCount - 1; retryCount++)
            {
                result = await db.QueryAsync<T>(sql, param, commandType: commandType);
                if (result != null)
                {
                    return result;
                }
                else if ((retryCount + 1) < RetryCount)
                {
                    _log.Warn($"Retry [{retryCount}]:" + sql);
                    System.Threading.Thread.Sleep(RetryInterval);
                }
            }
            return result;
        }
    }
}
