using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using LegacyGateway.Model;
using LegacyGateway.Utilities;
using RestSharp;
using TableDependency.Enums;
using Topshelf.Logging;
using System.Collections.Generic;
using System.Linq;

namespace LegacyGateway.Api
{
    public class CustomFieldValueApi : IProvisionable<CustomFieldValue>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();
        private int[] fieldTypesToWatch = new int[] {
            1, // String
            2, // Text Block
            3, // Drop Down List
            4, // Integer
            6, // Date
            7, // Decimal
            9, // Multiple Pick List
            18 // File
        };

        private int[] entityTypesToWatch = new int[]
        {
            4, // Company
            5, // Contact
            39 // BidSubmission
        };

        public bool Push(CustomFieldValue entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if (changeType == ChangeType.Insert || changeType == ChangeType.Update || changeType == ChangeType.Delete)
            {
                LegacyGateway.DTO.CustomFieldData customField = GetCustomField(entity);
                if (customField != null)
                {
                    entity.FieldType = customField.FieldType;
                    if (Array.IndexOf(entityTypesToWatch, customField.ObjectType) > -1 || entityTypesToWatch.Intersect(customField.AppliesTo).Any())
                    {
                        if (Array.IndexOf(fieldTypesToWatch, customField.FieldType) > -1)
                        {
                            if (customField.IsCompany)
                            {
                                entity.Type = "Company";
                            }
                            else if(customField.IsContact)
                            {
                                entity.Type = "Contact";
                            }
                            else if(customField.ObjectType == 39)
                            {
                                entity.Type = "BidSubmission";
                            }

                            if (!String.IsNullOrEmpty(entity.Type))
                            {
                                Method method = changeType == ChangeType.Delete ? Method.DELETE : Method.POST;
                                RestClientHelper.Send(entity.GetType().Name, "custom_field_values", method, entity);
                            }
                        }
                    }
                }
            }
            return true;
        }
        private LegacyGateway.DTO.CustomFieldData GetCustomField(CustomFieldValue entity)
        {
            DTO.CustomFieldData customField;
            using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
            {
                var sp_params = new DynamicParameters();
                sp_params.Add("@customFieldID", entity.CustomFieldID);
                sp_params.Add("@CustomFieldValueID", entity.CustomFieldValueID);
                using (var multi = db.QueryMultiple($"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetCustomField]", sp_params, commandType: CommandType.StoredProcedure))
                {
                    customField = multi.ReadFirstOrDefault<DTO.CustomFieldData>();
                    if (customField != null)
                    {
                        customField.AppliesTo = new int[] { };
                        customField.AppliesTo = multi.Read<int>().ToArray();
                    }
                }
            }
            return customField;
        }
    }
}
