using System;
using System.Data.SqlClient;

namespace Transact
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string connectionString = @"Data Source=(local)\SS2019;Initial Catalog=TRANSACT_DEV;User ID=tct;Password=tct";
            string sql = "SELECT TOP 1 @TEST_VAR AS TEST, * FROM TCT.GET_EXECUTED_QUERIES() ORDER BY last_execution_time DESC";
            sql = "INSERT INTO TCT.IMPORT_FILE_LOG VALUES (1, 1, 'test', GETDATE(), 1, 'test remarks')";

            using SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@TEST_VAR", "Hi");
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(String.Format("{0} - {1}",
                    reader["TEST"],
                    reader["query_text"]));
            }
        }
    }
}
