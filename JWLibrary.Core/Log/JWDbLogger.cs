using System.Data.SqlClient;

namespace JWLibrary.Core.Log
{
    public class JWDbLogger : IJWLogger
    {
        private SqlConnection _con;
        public JWDbLogger(string conString)
        {
            _con = new SqlConnection(conString);
        }

        public void WriteLog(string strLog, LogCode logCode)
        {
            try
            {
                _con.Open();

                var transaction = _con.BeginTransaction();

                var command = _con.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "";
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter {ParameterName = "LogText", Value = strLog },
                    new SqlParameter {ParameterName = "LogCode", Value = logCode.ToString() }
                });
                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                throw;
            }
            finally
            {
                _con.Close();
            }
        }
    }
}
