using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace JWLibrary.StaticMethod
{   
    /// <summary>
    /// MSSQL Database Client Connection Information
    /// </summary>
    public class MSSqlDataBase : IDataBaseProvider
    {
        public SqlConnection Connection {get; private set;}
        public MSSqlDataBase(JDataBase db)
        {
            this.Connection = new SqlConnection(db.ConnectionString);
        }
    }

    /// <summary>
    /// MYSQL Database Client Connection Information
    /// </summary>
    public class MYSqlDataBase : IDataBaseProvider
    {
        public MySqlConnection Connection {get; private set;}
        public MYSqlDataBase(JDataBase db) {
            this.Connection = new MySqlConnection(db.ConnectionString);
        }
    }

    /// <summary>
    /// Database Information Master
    /// </summary>
    public class JDataBase : IDataBase {
        public string ConnectionString {get; private set;}
        public JDBType DBType {get; private set;}
        private IConfiguration configuration;

        public JDataBase(){
            ServiceCollection serviceCollection = new ServiceCollection();
            InitConfig(serviceCollection);
        }

        public JDataBase(string connectionString, JDBType dbType) {
            this.ConnectionString = connectionString;
            this.DBType = dbType;
        }

        private void InitConfig(IServiceCollection serviceCollection)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var section = configuration.GetSection("Sectionofsettings");
            this.ConnectionString = section.GetValue<string>("MSSQL");
            this.DBType = JDBType.MSSQL;

            if(this.ConnectionString.jIsNullOrEmpty()) {
                this.ConnectionString = section.GetValue<string>("MYSQL");
                this.DBType = JDBType.MYSQL;

                if(this.ConnectionString.jIsNullOrEmpty()) throw new Exception("Not init db connection.");
            }
        }

        public enum JDBType {
            MSSQL,
            MYSQL
        }
    }

    public class JDataBaseFactory
    {
        public static IDataBaseProvider Create(JDataBase jDataBase)
        {
            if(jDataBase.DBType == JDataBase.JDBType.MSSQL) return new MSSqlDataBase(jDataBase);
            else if(jDataBase.DBType == JDataBase.JDBType.MYSQL) return new MYSqlDataBase(jDataBase);
            else {
                throw new NotImplementedException();
            }
        }
    }
}