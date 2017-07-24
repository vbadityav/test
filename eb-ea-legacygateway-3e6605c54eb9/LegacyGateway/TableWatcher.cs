using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.Enums;
using TableDependency.SqlClient;
using Topshelf.Logging;
using LegacyGateway.Api;
using System.ComponentModel.DataAnnotations.Schema;
using LegacyGateway.Utilities;
using System.Reflection;

namespace LegacyGateway
{

    public interface ITableWatcher
    {
        void Start();
        void Stop();
    }

    public class TableWatcher<T, U> : IDisposable, ITableWatcher where U : class, IProvisionable<T>, new() where T : class
    {
        private static readonly LogWriter _log = HostLogger.Get<LegacyGatewayService>();
        private readonly SqlTableDependency<T> _dependency;
        private U api;
        private string tableName;
        public TableWatcher(DatabaseInfo dbInfo, bool autoStart = false)
        {
            TableAttribute attribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            tableName = attribute.Name;
            api = new U()
            {
                DatabaseInfo = dbInfo
            };
            if (dbInfo.IsAppDefined)
            {
                _dependency = new SqlTableDependency<T>(api.DatabaseInfo.GetAppConnectionString());
            }
            else
            {
                _dependency = new SqlTableDependency<T>(api.DatabaseInfo.GetCoreConnectionString());
            }
            _dependency.OnChanged += _dependency_OnChanged;
            _dependency.OnError += _dependency_OnError;
            _dependency.OnStatusChanged += _dependency_OnStatusChanged;

            if (autoStart)
            {
                _dependency.Start();
                Console.WriteLine(tableName);
            }
        }

        public void Start()
        {
            _dependency.Start();
        }

        public void Stop()
        {
            _dependency.Stop();
        }

        private void _dependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            _log.Error($"{tableName} - {e.Message}");
        }

        private void _dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<T> e)
        {
            if (e.ChangeType != ChangeType.None)
            {

                //_log.Info($"{e.Entity.GetType().Name}: {e.ChangeType}");
                try
                {
                    api.Push(e.Entity, e.ChangeType);
                }
                catch (Exception ex)
                {
                    _log.Error($"Error on: {tableName}");
                    _log.Error(ex);
                }
                // Hit provisioning api end point for upsert
            }
        }

        private void _dependency_OnStatusChanged(object sender, TableDependency.EventArgs.StatusChangedEventArgs e)
        {
            TableAttribute attribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            _log.Debug($"{tableName} - {e.Status}");
        }

        public void Dispose()
        {
            _dependency.Stop();
        }
    }
}
