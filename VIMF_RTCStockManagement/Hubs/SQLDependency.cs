using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Xml.Linq;
using VIMF_RTCStockManagement.Common;
using static VIMF_RTCStockManagement.Common.SqlDependencyEx;

namespace VIMF_RTCStockManagement.Hubs
{
    public class SqlDependencyService
    {
        private readonly string _connectionString;
        private readonly IHubContext<NotificationHub> _hubContext;
        private SqlDependencyEx _dependency;
        public bool isStopped = false;

        public SqlDependencyService(IConfiguration configuration, IHubContext<NotificationHub> hubContext, ConnectionStringProvider conn)
        {
            _connectionString = conn.DefaultConnectionString;
            _hubContext = hubContext;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);
            string dbName = builder.InitialCatalog;
            // Note: Identity of sqldenpendency MUST be unique across all apps, check DB if you're not sure which to use.
            _dependency = new SqlDependencyEx(_connectionString, dbName, "ImportWarehouse", identity: 1);
            _dependency.TableChanged += TableChangedHandler;
            //_dependency.Stop();
            _dependency.Start();
        }

        private void TableChangedHandler(object? sender, TableChangedEventArgs e)
        {
            try
            {
                var (inserted, deleted) = ToInsertedDeleted(e.Data);
                var result = new
                {
                    NotificationType = e.NotificationType.ToString(),
                    Inserted = inserted?.ToString().Replace("<inserted>", "").Replace("</inserted>", ""),
                    Deleted = deleted?.ToString().Replace("<deleted>", "").Replace("</deleted>", ""),
                };
                string resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
                _hubContext.Clients.All.SendAsync("Refresh", resultJson).Wait();
            }
            catch (Exception ex)
            {
                _hubContext.Clients.All.SendAsync("Error", ex.Message).Wait();
            }
        }

        public void Stop()
        { _dependency.Stop(); }

        public static (XElement? Inserted, XElement? Deleted) ToInsertedDeleted(XElement rootElement)
        {
            var inserted = rootElement.Element("inserted");
            var deleted = rootElement.Element("deleted");
            return (inserted, deleted);
        }
    }
}