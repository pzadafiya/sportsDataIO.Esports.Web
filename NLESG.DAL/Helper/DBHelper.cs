using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace NLESG.DAL.Helper
{
    public class DBHelper
    {
        public static bool ExecuteSPForBulkUpdate(string ConnectionString, string SPName, DataTable dtTable, string ParameterName = "")
        {
            try
            {
                using (SqlConnection objSqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand objSqlCommand = new SqlCommand(SPName, objSqlConnection))
                    {

                        objSqlCommand.CommandType = CommandType.StoredProcedure;
                        // Use Common parameter name for all api store procedure to bulk update of the data
                        objSqlCommand.Parameters.AddWithValue(string.IsNullOrWhiteSpace(ParameterName) ? "@DataCollection" : ParameterName, dtTable); objSqlCommand.CommandTimeout = int.MaxValue;
                        objSqlConnection.Open();
                        objSqlCommand.ExecuteScalar();
                        objSqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLogHelper.LogErrorInDB(ex, "ExecuteSPForBulkUpdate(sp=" + SPName);
            }
            return true;
        }
    }
}
