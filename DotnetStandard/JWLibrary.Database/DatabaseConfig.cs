using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Database {
    public class DatabaseConfig {
        public static System.Data.IDbConnection DB_CONNECTION { get; private set; }
        public static void Configure() {
            var constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=acc;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            DB_CONNECTION = new System.Data.SqlClient.SqlConnection(constr);
            DB_CONNECTION.Open();
            DB_CONNECTION.Close();

        }
    }
}
