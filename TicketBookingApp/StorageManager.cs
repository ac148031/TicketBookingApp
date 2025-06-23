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

        private readonly SqlConnection? connection;

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
            Thread loading = new(() => ConsoleView.LoadingText("Loading Data"));

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

        private static void AddInsertProperties(string? property, string propertyName, ref string values, ref string columns, ref bool notNull)
        {
            if (!string.IsNullOrEmpty(property))
            {
                values += values[^1] == '(' ? $"'{property}'" : $", '{property}'";
                columns += columns[^1] == '(' ? $"{propertyName}" : $", {propertyName}";
                notNull = true;
            }
        }

        private static void AddUpdateProperties(string? property, string propertyName, ref string setClause, ref bool notNull)
        {
            if (!string.IsNullOrEmpty(property))
            {
                setClause += setClause.Length == 0 ? $"{propertyName} = '{property}'" : $", {propertyName} = '{property}'";
                notNull = true;
            }
        }

        public List<City>? Cities(SQLAction SQLAction, City? cityOne = null, City? cityTwo = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<City> cities = [];
                    sqlString = "SELECT * FROM concerts.tblCities";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        using SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cities.Add(new City(
                                reader.GetInt32(reader.GetOrdinal("cityId")),
                                reader.GetString(reader.GetOrdinal("cityName"))));
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

        public List<Customer>? Customers(SQLAction SQLAction, string whereClause = "", Customer? insertCustomer = null)
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
                    sqlString = "INSERT INTO sales.tblCustomers ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertCustomer?.CustomerFirstName, "customerFirstName", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomer?.CustomerLastName, "customerLastName", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomer?.CustomerPhone, "customerPhone", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomer?.CustomerEmail, "customerEmail", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomer?.CustomerUsername, "customerUsername", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomer?.CustomerPassword, "customerPassword", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    //Console.WriteLine(sqlString);

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM _ WHERE";
                    using (SqlCommand cmd = new(sqlString, connection))
                    {

                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE sales.tblCustomers ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertCustomer?.CustomerFirstName, "customerFirstName", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomer?.CustomerLastName, "customerLastName", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomer?.CustomerPhone, "customerPhone", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomer?.CustomerEmail, "customerEmail", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomer?.CustomerUsername, "customerUsername", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomer?.CustomerPassword, "customerPassword", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    Console.WriteLine(sqlString);

                    //using (SqlCommand cmd = new(sqlString, connection))
                    //{
                    //    cmd.ExecuteNonQuery();
                    //}
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }
    }
}
