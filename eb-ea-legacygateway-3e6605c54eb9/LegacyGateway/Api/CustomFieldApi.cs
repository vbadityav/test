using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TableDependency.Enums;
using Topshelf.Logging;

namespace LegacyGateway.Api
{
    public class CustomFieldApi : IProvisionable<CustomField>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        private int[] entityTypesToWatch = new int[]
        {
            -1,
            4, // Company
            5, // Contact
            39 // BidSubmission
        };

        public bool Push(CustomField entity, ChangeType changeType)
        {
            if (Array.IndexOf(entityTypesToWatch, entity.ObjectType) > -1)
            {
                _log.Info($"{entity.GetType().Name} - {changeType}");
                if (changeType == ChangeType.Insert || changeType == ChangeType.Update)
                {
                    if (entity.ObjectType == -1)
                    {
                        SendForGlobal(entity);
                    }
                    else
                    {
                        if (entity.ObjectType == 4)
                        {
                            entity.Type = "Company";
                        }
                        else if (entity.ObjectType == 5)
                        {
                            entity.Type = "Contact";
                        }
                        else if (entity.ObjectType == 39)
                        {
                            entity.Type = "BidSubmission";
                        }
                        RestClientHelper.Send(entity.GetType().Name, "custom_fields", Method.POST, entity);
                    }
                }
                else if (changeType == ChangeType.Delete)
                {
                    RestClientHelper.Send(entity.GetType().Name, "custom_fields", Method.DELETE, entity);
                }
            }
            return true;
        }

        public void SendForGlobal(CustomField entity)
        {
            string sql =
                "SELECT" +
                "  AppliesTo " +
                $"FROM {DatabaseInfo.AppDatabase}.dbo.daCustomFieldAppliesTo (NOLOCK)" +
                "WHERE CustomFieldID = @CustomFieldID;";

            List<int> appliesTo = new List<int>();
            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                var sp_params = new DynamicParameters();
                sp_params.Add("@CustomFieldID", entity.CustomFieldID);
                appliesTo = DatabaseHelper.Query<int>(db, sql, sp_params).Result.ToList();
            }

            foreach (int type in entityTypesToWatch.Intersect(appliesTo))
            {
                if (type == 4)
                {
                    entity.Type = "Company";
                    RestClientHelper.Send(entity.GetType().Name, "custom_fields", Method.POST, entity);
                }
                else if (type == 5)
                {
                    entity.Type = "Contact";
                    RestClientHelper.Send(entity.GetType().Name, "custom_fields", Method.POST, entity);
                }
            }
        }
    }
}
