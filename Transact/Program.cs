using DatabaseManager;

using System;
using System.Data;
using System.Data.SqlClient;

namespace Transact
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();

            UpdateExample();
            SelectExample();
            DeleteExample();
            InsertExample();

            Stop();
        }

        private static void Start()
        {
            Console.WriteLine("Starting Transact");
        }

        private static void Stop()
        {
            Console.WriteLine("Stopping Transact");
        }

        private static void SelectExample()
        {
            SelectExample_DataReader();
            SelectExample_DataAdapter();
        }

        /// <summary>
        /// Selects records from the Database using a DataReader.
        /// 
        /// Retrieves records one by one. This means only one record is held in memory at a time, which is more memory efficient.
        /// 
        /// Maintains an active connection with the database while reading. This means the connection to the database is open for the lifetime of the current reading.
        /// </summary>
        private static void SelectExample_DataReader()
        {
            string connectionString = @"Data Source=(local)\SS2019;Initial Catalog=TRANSACT_DEV;User ID=tct;Password=tct";
            string sql = "SELECT TOP 2 * FROM [TCT].[TRANSACT]";

            using SqlConnection connection = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            Console.WriteLine("Select using DataReader");

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Processing Record");

                // Process all fields in the record
                Console.WriteLine("Record Values (iterate - foreach)");
                object[] record = new object[reader.FieldCount];
                reader.GetValues(record);
                ProcessRecord_Foreach(record);

                // Process all fields in the record by index
                Console.WriteLine("Record Values (iterate - index)");
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    Console.WriteLine(string.Format("{0}: {1}", reader.GetName(i), reader[i]));
                }
                Console.WriteLine();

                // Process specific fields in the record
                Console.WriteLine("Record Values (index by name)");
                Console.WriteLine(string.Format("{0} - {1}", "ID", reader["ID"]));
                Console.WriteLine(string.Format("{0} - {1}", "DESCRIPTION", reader["DESCRIPTION"]));
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Selects records from the Database using a DataAdapter.
        /// 
        /// Retrieves records all at once. This means all records are held in memory.
        /// 
        /// Closes the connection with the database once the records are retrieved.
        /// </summary>
        private static void SelectExample_DataAdapter()
        {
            string sql = "SELECT TOP 2 * FROM [TCT].[TRANSACT]";
            using DataSet dataSet = DatabaseHelper.Select(sql);

            Console.WriteLine(string.Format("Processing DataSet: {0} Tables", dataSet.Tables.Count));
            foreach (DataTable table in dataSet.Tables)
            {
                Console.WriteLine(string.Format("Processing Table {0}: {1} Rows", table.TableName, table.Rows.Count));
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine("Processing Row");

                    // Process all fields in the record
                    Console.WriteLine("Row Values (iterate - foreach)");
                    ProcessRecord_Foreach(row.ItemArray);

                    // Process all fields in the record by index
                    Console.WriteLine("Row Values (iterate - index)");
                    for (int i = 0; i < row.Table.Columns.Count; ++i)
                    {
                        Console.WriteLine(string.Format("{0}: {1}", row.Table.Columns[i].ColumnName, row[i]));
                    }
                    Console.WriteLine();

                    // Process specific fields in the record
                    Console.WriteLine("Row Values (index by name)");
                    Console.WriteLine(string.Format("{0} - {1}", "ID", row["ID"]));
                    Console.WriteLine(string.Format("{0} - {1}", "DESCRIPTION", row["DESCRIPTION"]));
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Updates records in the Database using a DataAdapter.
        /// </summary>
        private static void UpdateExample()
        {
            Console.WriteLine("Update using DataAdapter");
            string sql = "UPDATE [TCT].[TRANSACT] SET DESCRIPTION = CONCAT(DESCRIPTION, ', now NEW and IMPROVED')";
            int affectedRecords = DatabaseHelper.Update(sql);
            Console.WriteLine(string.Format("Updated {0} Rows", affectedRecords));
            Console.WriteLine();
        }

        /// <summary>
        /// Inserts records into the Database using a DataAdapter.
        /// </summary>
        private static void InsertExample()
        {
            Console.WriteLine("Insert using DataAdapter");
            string sql = "INSERT INTO [TCT].[TRANSACT] VALUES(2, 1, 1, 100010, GETUTCDATE(), 1, 1, 'Generic Description', 0, 1000, 1000, 0, 1, GETUTCDATE(), 1, GETUTCDATE())";
            int affectedRecords = DatabaseHelper.Insert(sql);
            Console.WriteLine(string.Format("Inserted {0} Rows", affectedRecords));
            Console.WriteLine();
        }

        /// <summary>
        /// Deletes records from the Database using a DataAdapter.
        /// </summary>
        private static void DeleteExample()
        {
            Console.WriteLine("Delete using DataAdapter");
            string sql = "WITH CTE AS (SELECT TOP 1 * FROM [TCT].[TRANSACT] ORDER BY ID DESC)\r\nDELETE FROM CTE";
            //string sql = "DELETE FROM [TCT].[TRANSACT] WHERE ID IN (SELECT TOP 2 ID FROM [TCT].[TRANSACT] ORDER BY ID DESC)";
            int affectedRecords = DatabaseHelper.Delete(sql);
            Console.WriteLine(string.Format("Deleted {0} Rows", affectedRecords));
            Console.WriteLine();
        }

        private static void ProcessRecord_Foreach(object[] record)
        {
            foreach (object field in record)
            {
                Console.WriteLine(string.Format("{0}", field));
            }
            Console.WriteLine();
        }
    }
}