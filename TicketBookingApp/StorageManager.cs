using Microsoft.Data.SqlClient;
using System.Data;

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

        public List<City>? Cities(SQLAction SQLAction, City? cityOne = null, City? cityTwo = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<City> cities = new();
                    sqlString = "SELECT * FROM ";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int cityId = Convert.ToInt32(reader["cityId"]);
                                string? cityName = reader["cityName"].ToString();
                                cities.Add(new City(cityId, cityName ?? throw new Exception("Null City Name")));
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
    }
}
