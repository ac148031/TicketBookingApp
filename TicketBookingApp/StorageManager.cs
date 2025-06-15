using Microsoft.Data.SqlClient;
using System.Data;
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public class StorageManager
    {
        public enum SQLAction
        {
            Select = 0,
            Insert = 1,
            Delete = 2,
            Update = 3
        }

        private SqlConnection? connection;

        public StorageManager(string connectionString)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connection Successful.");
            }
            catch (SqlException e)
            {
                Console.WriteLine("Connection Unsuccessful.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured while attemping to secure connection.");
                Console.WriteLine(e.Message);
            }
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Connection Closed.");
            }
        }

        public void Setup()
        {
            void LoadingText()
            {
                Console.CursorVisible = false;
                try
                {
                    string[] dotSequence = ["   ", ".  ", ".. ", "..."];
                    string message = "Loading Data";

                    (int left, int top) = Console.GetCursorPosition();

                    for (int i = 0; true; i++)
                    {
                        if (i >= 4) i = 0;

                        Console.SetCursorPosition(left, top);

                        Console.Write(message + dotSequence[i]);

                        Thread.Sleep(250);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Console.CursorVisible = true;
                    Console.WriteLine();
                }

                Console.CursorVisible = true;
                Console.WriteLine();
            }

            Thread loading = new(new ThreadStart(LoadingText));

            loading.Start();

            // AppDomain.CurrentDomain.BaseDirectory gives the path to the bin directory of the project.
            string GetDirectory(string file) => Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..",
                "..",
                "..",
                "..",
                "TicketBookingDatabase",
                "TicketBookingDatabase",
                file);

            List<string> batches =
            [
                .. File.ReadAllText(GetDirectory("qryTableAdd.sql")).Split("GO", StringSplitOptions.RemoveEmptyEntries),
                .. File.ReadAllText(GetDirectory("qryLoadData.sql")).Split("GO", StringSplitOptions.RemoveEmptyEntries),
                File.ReadAllText(GetDirectory("qryLoadCustomerData.sql")),
                File.ReadAllText(GetDirectory("qryLoadCustomerAddresses.sql")),
                File.ReadAllText(GetDirectory("qryLoadSalesData.sql")),
            ];

            // Runs all SQL commands in the batches.
            foreach (var batch in batches)
            {
                SqlCommand cmd = new(batch, connection);
                cmd.ExecuteNonQuery();
            }

            loading.Interrupt();
        }

        public List<City>? Cities(SQLAction SQLAction, City? cityOne = null, City? cityTwo = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<City> cities = new();
                    sqlString = "SELECT * FROM concerts.tblCities";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cities.Add(new City(
                                    reader.GetInt32(reader.GetOrdinal("cityId")),
                                    reader.GetString(reader.GetOrdinal("cityName"))));
                            }
                        }
                    }
                    return cities;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO _ VALUES ";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM _ WHERE";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE _ SET ";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<Customer>? Customers(SQLAction SQLAction, string whereClause = "", Customer? customerOne = null, Customer? customerTwo = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<Customer> customers = new();
                    sqlString = "SELECT * FROM sales.tblCustomers " + whereClause;

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Customer(
                                    reader.GetInt32(reader.GetOrdinal("customerId")),
                                    reader.GetString(reader.GetOrdinal("customerFirstName")),
                                    reader.GetString(reader.GetOrdinal("customerLastName")),
                                    reader.GetString(reader.GetOrdinal("customerPhone")),
                                    reader.GetString(reader.GetOrdinal("customerEmail")),
                                    reader.GetString(reader.GetOrdinal("customerUsername")),
                                    reader.GetString(reader.GetOrdinal("customerPassword"))));
                            }
                        }
                    }
                    return customers;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO _ VALUES ";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM _ WHERE";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE _ SET ";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }
    }
}
