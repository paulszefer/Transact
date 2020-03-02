using System;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseManager
{
    public class DatabaseHelper
    {
        private static readonly string CONNECTION_STRING = @"Data Source=(local)\SS2019;Initial Catalog=TRANSACT_DEV;User ID=tct;Password=tct";
        private static readonly SqlConnection CONNECTION = new SqlConnection(CONNECTION_STRING);

        private DatabaseHelper() { }

        public static DataSet Select(string sql)
        {
            PrepareConnection();

            DataSet dataSet = new DataSet();
            using (SqlCommand command = new SqlCommand(sql, CONNECTION))
            {
                using SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(dataSet);
            }

            DisposeConnection();

            return dataSet;
        }

        public static int Update(string sql)
        {
            PrepareConnection();

            int affectedRows;
            using (SqlCommand command = new SqlCommand(sql, CONNECTION))
            {
                using SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.UpdateCommand = command;
                affectedRows = adapter.UpdateCommand.ExecuteNonQuery();
            }

            DisposeConnection();

            return affectedRows;
        }

        public static int Insert(string sql)
        {
            PrepareConnection();

            int affectedRows;
            using (SqlCommand command = new SqlCommand(sql, CONNECTION))
            {
                using SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = command;
                affectedRows = adapter.InsertCommand.ExecuteNonQuery();
            }

            DisposeConnection();

            return affectedRows;
        }

        public static int Delete(string sql)
        {
            PrepareConnection();

            int affectedRows;
            using (SqlCommand command = new SqlCommand(sql, CONNECTION))
            {
                using SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.DeleteCommand = command;
                affectedRows = adapter.DeleteCommand.ExecuteNonQuery();
            }

            DisposeConnection();

            return affectedRows;
        }

        private static void PrepareConnection()
        {
            if (CONNECTION.State != ConnectionState.Open)
            {
                CONNECTION.Open();
            }
        }

        private static void DisposeConnection()
        {
            CONNECTION.Close();
        }
    }
}
