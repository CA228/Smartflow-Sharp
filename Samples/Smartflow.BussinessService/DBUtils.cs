using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Smartflow.BussinessService
{
    public class DBUtils
    {
        static DBUtils()
        {
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
        }


        public static IDbConnection CreateConnection()
        {
            IConfigurationSection section = GlobalObjectService.Configuration.GetSection("ConnectionSetting");
            ConnectionSetting connectionSetting = section.Get<ConnectionSetting>();
            IDbConnection connection = DbProviderFactories.GetFactory(connectionSetting.ProviderName).CreateConnection();
            connection.ConnectionString = connectionSetting.ConnectionString;
            return connection;
        }
    }
}
