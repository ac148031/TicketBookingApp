using Microsoft.Data.SqlClient;
using System.Data;

namespace TicketBookingApp
{
    public class StorageManager
    {
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

        public List<City> CitiesSelect()
        {
            List<City> cities = new List<City>();
            string sqlString = "SELECT * FROM ";

            using (SqlCommand cmd = new SqlCommand(sqlString, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cityId = Convert.ToInt32(reader["cityId"]);
                        string cityName = reader["cityName"].ToString();
                        cities.Add(new City(cityId, cityName));
                    }
                }
            }
            return cities;
        }

        public void CitiesInsert(City city)
        {
            string sqlString = "INSERT INTO _ VALUES ";
            using (SqlCommand cmd = new SqlCommand(sqlString, connection))
            {

            }
        }

        public void CitiesDelete(City city)
        {
            string sqlString = "DELETE FROM _ WHERE";
            using (SqlCommand cmd = new SqlCommand(sqlString, connection))
            {

            }
        }

        public void CitiesUpdate(City oldCity, City newCity)
        {
            string sqlString = "UPDATE _ SET ";
            using (SqlCommand cmd = new SqlCommand(sqlString, connection))
            {

            }
        }
    }
}
