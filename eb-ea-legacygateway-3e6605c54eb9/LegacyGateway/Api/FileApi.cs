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
    public class FileApi : IProvisionable<File>
    {
        public DatabaseInfo DatabaseInfo { get; set; }
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();

        public bool Push(File entity, ChangeType changeType)
        {
            _log.Info($"{entity.GetType().Name} - {changeType}");
            if(changeType == ChangeType.Insert || changeType == ChangeType.Update)
            {
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString)){
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@fileID", entity.FileID);
                    FileData fileData = DatabaseHelper.QueryFirstOrDefault<FileData>(db, $"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetFileData]", sp_params, commandType: CommandType.StoredProcedure).Result;
                    if (fileData != null)
                    {
                        entity.ParentID = fileData.ParentID;
                        entity.DateCreated = fileData.DateCreated;
                        entity.InvitationID = fileData.InvitationID;
                        entity.BidID = fileData.BidID;
                    }
                }

                RestClientHelper.Send(entity.GetType().Name, "files", Method.POST, entity);
                
                using (IDbConnection db = new SqlConnection(DatabaseInfo.ConnectionString))
                {
                    var sp_params = new DynamicParameters();
                    sp_params.Add("@fileID", entity.FileID);
                    List<FilePath> paths = DatabaseHelper.Query<FilePath>(db, $"{DatabaseInfo.AppDatabase}.dbo.[Gateway_GetFilePaths]", sp_params, commandType: CommandType.StoredProcedure).Result.ToList();
                    foreach (FilePath path in paths)
                    {
                        RestClientHelper.Send(entity.GetType().Name + " - File Path", "file_paths", Method.POST, path);
                    }
                }

            }
            return true;
        }
    }

    public class FileData
    {
        public Guid ParentID { get; set; }
        public Guid? InvitationID { get; set; }
        public Guid? BidID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
