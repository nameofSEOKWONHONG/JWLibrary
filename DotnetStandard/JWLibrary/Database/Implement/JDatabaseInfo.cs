using JWLibrary.Core;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JWLibrary.Database {

    /// <summary>
    ///     Database Information Master
    /// </summary>
    internal class JDatabaseInfo {
        private IConfiguration configuration;

        public JDatabaseInfo() {
            var serviceCollection = new ServiceCollection();
            InitConfig(serviceCollection);
        }

        public Dictionary<string, IDbConnection> ConKeyValues { get; } = new Dictionary<string, IDbConnection>
        {
            {"MSSQL", null},
            {"MYSQL", null}
        };

        public Dictionary<string, ILiteDatabase> NoSqlConKeyValues { get; } = new Dictionary<string, ILiteDatabase>
        {
            {"LITEDB", null}
        };

        private void InitConfig(IServiceCollection serviceCollection) {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var section = configuration.GetSection("DbConnections");

            ConKeyValues.jForEach(item => {
                if (item.Key.jEquals("MSSQL"))
                    ConKeyValues[item.Key] = new SqlConnection(section.GetValue<string>(item.Key));
                else if (item.Key.jEquals("MYSQL"))
                    ConKeyValues[item.Key] = new MySqlConnection(section.GetValue<string>(item.Key));
                else if (item.Key.jEquals("LITEDB"))
                    NoSqlConKeyValues[item.Key] = new LiteDatabase(section.GetValue<string>(item.Key));
            });
        }
    }
}