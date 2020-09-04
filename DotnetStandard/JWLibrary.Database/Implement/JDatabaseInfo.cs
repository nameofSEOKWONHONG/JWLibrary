using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using JWLibrary.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace JWLibrary.Database
{
    /// <summary>
    /// Database Information Master
    /// </summary>
    class JDatabaseInfo {
        private IConfiguration configuration;

        public Dictionary<string, IDbConnection> ConKeyValues = new Dictionary<string, IDbConnection>() {
            {"MSSQL", null },
            {"MYSQL", null }
        };

        public JDatabaseInfo(){
            ServiceCollection serviceCollection = new ServiceCollection();
            InitConfig(serviceCollection);
        }

        private void InitConfig(IServiceCollection serviceCollection)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var section = configuration.GetSection("DbConnections");

            this.ConKeyValues.jForEach(item => {
                if(item.Key.jEquals("MSSQL")) {
                    ConKeyValues[item.Key] = new SqlConnection(section.GetValue<string>(item.Key));
                }
                else if (item.Key.jEquals("MYSQL")) {
                    ConKeyValues[item.Key] = new MySqlConnection(section.GetValue<string>(item.Key));
                }
            });
        }
    }


}