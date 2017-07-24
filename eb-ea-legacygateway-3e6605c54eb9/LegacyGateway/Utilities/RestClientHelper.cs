using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Utilities
{
    public class RestClientHelper
    {
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public static bool Send(string entityName, string endpoint, Method method, object entity)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["ProvisioningEndpoint"]);
            var request = new RestSharp.Newtonsoft.Json.RestRequest(endpoint, method);

            var setting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var content = JsonConvert.SerializeObject(entity, setting);
            _log.Info($"{entityName} - {content}");
            request.AddParameter("application/json", content, ParameterType.RequestBody);

            var response = client.Execute<ApiResponse>(request);
            return LogResponse(entityName, content, response);
        }

        private static bool LogResponse(string entityName, string content, IRestResponse<ApiResponse> response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                {
                    _log.Error($"{entityName} - {response.ErrorMessage}");
                }
                if (!(response.Data is null))
                {
                    try
                    {
                        _log.Info($"{entityName} - {response.Data.Message}");
                    }
                    catch { }
                }
                return true;
            }
            else
            {
                _log.Error($"{entityName} - {content}");
                if (!String.IsNullOrWhiteSpace(response.ErrorMessage))
                {
                    _log.Error($"{entityName} - {response.ErrorMessage}");
                }
                if (!(response.Data is null))
                {
                    try
                    {
                        _log.Error($"{entityName} - {response.Data.Message}");
                    }
                    catch { }
                }
                return false;
            }
        }

        public static void Send(string entityName, string endpoint, Method method, object entity, Action<bool> callback)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["ProvisioningEndpoint"]);
            var request = new RestSharp.Newtonsoft.Json.RestRequest(endpoint, method);

            var setting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter()
                    {
                            DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal
                    }
                }
            };
            var content = JsonConvert.SerializeObject(entity, setting);
            _log.Info($"{entityName} - {content}");
            request.AddParameter("application/json", content, ParameterType.RequestBody);

            client.ExecuteAsync<ApiResponse>(request, response =>
            {
                bool result = LogResponse(entityName, content, response);
                callback(result);
            });
        }
    }
}
