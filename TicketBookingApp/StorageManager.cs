using Microsoft.Data.SqlClient;
using System.Data;
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public enum SQLAction
    {
        Select,
        Insert,
        Delete,
        Update
    }

    public class StorageManager
    {
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
            string prefix = values[^1] == '(' ? "" : ", ";
            if (DateOnly.TryParse(property, out _))
            {
                values += prefix + $"CONVERT( DATE, '{property}', 103)";
                columns += prefix + $"{propertyName}";
                notNull = true;
            }
            else if (TimeOnly.TryParse(property, out _))
            {
                values += prefix + $"CONVERT( TIME, '{property}')";
                columns += prefix + $"{propertyName}";
                notNull = true;
            }
            else if (!string.IsNullOrEmpty(property))
            {
                values += prefix + $"'{property}'";
                columns += prefix + $"{propertyName}";
                notNull = true;
            }
        }

        private static void AddUpdateProperties(string? property, string propertyName, ref string setClause, ref bool notNull)
        {
            string prefix = setClause.Length <= 4 ? "" : ", ";
            if (DateOnly.TryParse(property, out _))
            {
                setClause += prefix + $"{propertyName} = CONVERT( DATE, '{property}', 103)";
                notNull = true;
            }
            else if (TimeOnly.TryParse(property, out _))
            {
                setClause += prefix + $"{propertyName} = CONVERT( TIME, '{property}')";
                notNull = true;
            }
            if (!string.IsNullOrEmpty(property))
            {
                setClause += prefix + $"{propertyName} = '{property}'";
                notNull = true;
            }
        }

        public List<City>? Cities(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, City? insertCity = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<City> cities = new();
                    sqlString = "SELECT * FROM concerts.tblCities " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

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
                    sqlString = "INSERT INTO concerts.tblCities ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertCity?.CityName, "cityName", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM concerts.tblCities " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE concerts.tblCities ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertCity?.CityName, "cityName", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");

            }
        }

        public List<Customer>? Customers(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, Customer? insertCustomer = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<Customer> customers = new();
                    sqlString = "SELECT * FROM sales.tblCustomers " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            customers.Add(new Customer(
                                reader.GetInt32(reader.GetOrdinal("customerId")),
                                reader.GetString(reader.GetOrdinal("customerFirstName")),
                                reader.GetString(reader.GetOrdinal("customerLastName")),
                                reader.GetString(reader.GetOrdinal("customerPhone")),
                                reader.GetString(reader.GetOrdinal("customerEmail")),
                                reader.GetString(reader.GetOrdinal("customerUsername")),
                                reader.GetString(reader.GetOrdinal("customerPassword")),
                                reader.GetBoolean(reader.GetOrdinal("customerIsAdmin"))));
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
                    AddInsertProperties(Convert.ToInt32(insertCustomer?.CustomerIsAdmin).ToString(), "customerIsAdmin", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM sales.tblCustomers " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
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
                    AddUpdateProperties(Convert.ToInt32(insertCustomer?.CustomerIsAdmin).ToString(), "customerIsAdmin", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<FullCustomer>? FullCustomers(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<FullCustomer> fullCustomers = new();
                    sqlString = "SELECT cs.customerId, cs.customerFirstName, cs.customerLastName, cs.customerPhone, cs.customerEmail, cs.customerUsername, cs.customerPassword, cs.customerIsAdmin, STRING_AGG(ca.addressId, ':') AS addressIds, STRING_AGG(ca.streetAddress, ':') AS streetAddresses, STRING_AGG(ct.cityName, ':') AS cityNames, STRING_AGG(ca.postalCode, ':') AS postalCodes " +
                                "FROM sales.tblCustomers AS cs LEFT OUTER JOIN sales.tblCustomerAddresses AS ca ON cs.customerId = ca.customerId LEFT OUTER JOIN concerts.tblCities AS ct ON ct.cityId = ca.cityId "
                                + whereClause +
                                " GROUP BY cs.customerId, cs.customerFirstName, cs.customerLastName, cs.customerPhone, cs.customerEmail, cs.customerUsername, cs.customerPassword, cs.customerIsAdmin;";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int customerId = reader.GetInt32(reader.GetOrdinal("customerId"));

                            FullCustomer current = new FullCustomer(
                                customerId,
                                reader.GetString(reader.GetOrdinal("customerFirstName")),
                                reader.GetString(reader.GetOrdinal("customerLastName")),
                                reader.GetString(reader.GetOrdinal("customerPhone")),
                                reader.GetString(reader.GetOrdinal("customerEmail")),
                                reader.GetString(reader.GetOrdinal("customerUsername")),
                                reader.GetString(reader.GetOrdinal("customerPassword")),
                                reader.GetBoolean(reader.GetOrdinal("customerIsAdmin")));

                            string[] streetAddresses = reader.IsDBNull(reader.GetOrdinal("streetAddresses"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("streetAddresses")).Split(':');

                            string[] cityNames = reader.IsDBNull(reader.GetOrdinal("cityNames"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("cityNames")).Split(':');

                            string[] postalCodes = reader.IsDBNull(reader.GetOrdinal("postalCodes"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("postalCodes")).Split(':');

                            string[] addressIds = reader.IsDBNull(reader.GetOrdinal("addressIds"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("addressIds")).Split(':');


                            if (streetAddresses.Length != cityNames.Length && streetAddresses.Length != postalCodes.Length)
                            {
                                throw new Exception("Mismatching address data");
                            }

                            List<FullCustomerAddress> addresses = new();

                            for (int i = 0; i < streetAddresses.Length; i++)
                            {
                                addresses.Add(new FullCustomerAddress(
                                    int.Parse(addressIds[i]),
                                    customerId,
                                    streetAddresses[i],
                                    cityNames[i],
                                    postalCodes[i]
                                    ));
                            }

                            current.CustomerAddresses = addresses;

                            fullCustomers.Add(current);
                        }
                    }
                    return fullCustomers;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<CustomerAddress>? CustomerAddresses(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, CustomerAddress? insertCustomerAddress = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<CustomerAddress> customerAddresses = new();
                    sqlString = "SELECT * FROM sales.tblCustomerAddresses " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            customerAddresses.Add(new CustomerAddress(
                                reader.GetInt32(reader.GetOrdinal("addressId")),
                                reader.GetInt32(reader.GetOrdinal("customerId")),
                                reader.GetString(reader.GetOrdinal("streetAddress")),
                                reader.GetInt32(reader.GetOrdinal("cityId")),
                                reader.GetString(reader.GetOrdinal("postalCode"))));
                        }
                    }
                    return customerAddresses;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO sales.tblCustomerAddresses ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertCustomerAddress?.CustomerId.ToString(), "customerId", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomerAddress?.StreetAddress, "streetAddress", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomerAddress?.CityId.ToString(), "cityId", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertCustomerAddress?.PostalCode, "postalCode", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM sales.tblCustomerAddresses " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE sales.tblCustomerAddresses ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertCustomerAddress?.CustomerId.ToString(), "customerId", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomerAddress?.StreetAddress, "streetAddress", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomerAddress?.CityId.ToString(), "cityId", ref setClause, ref notNull);
                    AddUpdateProperties(insertCustomerAddress?.PostalCode, "postalCode", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<FullCustomerAddress>? FullCustomerAddresses(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<FullCustomerAddress> fullCustomerAddresses = new();
                    sqlString = "SELECT * FROM sales.tblCustomerAddresses " + whereClause + ";";

                    Dictionary<int, string> cities = Cities(SQLAction.Select)?.ToDictionary(city => city.CityId, city => city.CityName) ?? new();

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            FullCustomerAddress current = new FullCustomerAddress(
                                reader.GetInt32(reader.GetOrdinal("addressId")),
                                reader.GetInt32(reader.GetOrdinal("customerId")),
                                reader.GetString(reader.GetOrdinal("streetAddress")),
                                reader.GetString(reader.GetOrdinal("postalCode")));

                            int cityId = reader.GetInt32(reader.GetOrdinal("cityId"));
                            current.CityName = cities[cityId];

                            fullCustomerAddresses.Add(current);
                        }
                    }
                    return fullCustomerAddresses;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<Concert>? Concerts(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, Concert? insertConcert = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<Concert> concerts = new();
                    sqlString = "SELECT * FROM concerts.tblConcerts " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            concerts.Add(new Concert(
                                reader.GetInt32(reader.GetOrdinal("concertId")),
                                reader.GetString(reader.GetOrdinal("concertName")),
                                reader.GetString(reader.GetOrdinal("concertDescription")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("concertDate"))),
                                TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("concertTime"))),
                                reader.GetInt32(reader.GetOrdinal("concertAvailTickets")),
                                reader.GetDecimal(reader.GetOrdinal("concertTicketPrice")),
                                reader.GetInt32(reader.GetOrdinal("locationId"))));
                        }
                    }
                    return concerts;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO concerts.tblConcerts ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertConcert?.ConcertName, "concertName", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcert?.ConcertDescription, "concertDescription", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcert?.ConcertDate.ToString("dd/MM/yyyy"), "concertDate", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcert?.ConcertTime.ToString("HH:mm:ss"), "concertTime", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcert?.ConcertAvailTickets.ToString(), "concertAvailTickets", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcert?.ConcertTicketPrice.ToString(), "concertTicketPrice", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcert?.LocationId.ToString(), "locationId", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM concerts.tblConcerts " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE concerts.tblConcerts ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertConcert?.ConcertName, "concertName", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcert?.ConcertDescription, "concertDescription", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcert?.ConcertDate.ToString("dd/MM/yyyy"), "concertDate", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcert?.ConcertTime.ToString("HH:mm:ss"), "concertTime", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcert?.ConcertAvailTickets.ToString(), "concertAvailTickets", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcert?.ConcertTicketPrice.ToString(), "concertTicketPrice", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcert?.LocationId.ToString(), "locationId", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<FullConcert>? FullConcerts(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<FullConcert> concerts = new();
                    sqlString = "SELECT * FROM ("
                        + "SELECT c.concertId, c.concertName, c.concertDescription, c.concertDate, c.concertTime, c.concertAvailTickets, c.concertTicketPrice, l.locationId, l.locationName, l.locationAddress, l.locationCapacity, ct.cityId, ct.cityName, STRING_AGG(g.genreId, ':') AS genreIds, STRING_AGG(g.genreName, ':') AS genreNames, STRING_AGG(g.genreDescription, ':') AS genreDescriptions "
                        + "FROM concerts.tblConcerts AS c LEFT OUTER JOIN concerts.tblConcertGenres AS cg ON c.concertId = cg.concertId LEFT OUTER JOIN concerts.tblGenres AS g ON cg.genreId = g.genreId LEFT OUTER JOIN concerts.tblLocations AS l ON c.locationId = l.locationId LEFT OUTER JOIN concerts.tblCities AS ct ON l.cityId = ct.cityId "
                        + "GROUP BY c.concertId, c.concertName, c.concertDescription, c.concertDate, c.concertTime, c.concertAvailTickets, c.concertTicketPrice, l.locationId, l.locationName, l.locationAddress, l.locationCapacity, ct.cityId, ct.cityName "
                        + ") AS Concerts " + whereClause + " ;";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            FullConcert current = new FullConcert(
                                reader.GetInt32(reader.GetOrdinal("concertId")),
                                reader.GetString(reader.GetOrdinal("concertName")),
                                reader.GetString(reader.GetOrdinal("concertDescription")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("concertDate"))),
                                TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("concertTime"))),
                                reader.GetInt32(reader.GetOrdinal("concertAvailTickets")),
                                reader.GetDecimal(reader.GetOrdinal("concertTicketPrice")));

                            current.ConcertLocation = new FullLocation(
                                reader.GetInt32(reader.GetOrdinal("locationId")),
                                reader.GetString(reader.GetOrdinal("locationName")),
                                reader.GetString(reader.GetOrdinal("locationAddress")),
                                reader.GetString(reader.GetOrdinal("cityName")),
                                reader.GetInt32(reader.GetOrdinal("locationCapacity"))
                                );

                            current.GenreList = new List<Genre>();
                            string[] genreIds = reader.IsDBNull(reader.GetOrdinal("genreIds"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("genreIds")).Split(':');

                            string[] genreNames = reader.IsDBNull(reader.GetOrdinal("genreNames"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("genreNames")).Split(':');

                            string[] genreDescriptions = reader.IsDBNull(reader.GetOrdinal("genreDescriptions"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("genreDescriptions")).Split(':');


                            if (genreIds?.Length != genreNames?.Length || genreIds?.Length != genreDescriptions?.Length)
                                throw new Exception("Genre data is missing");

                            if (genreIds != null && genreNames != null && genreDescriptions != null)
                            {
                                for (int i = 0; i < genreIds.Length; i++)
                                {
                                    current.GenreList.Add(new Genre(
                                        int.Parse(genreIds[i]),
                                        genreNames[i],
                                        genreDescriptions[i]
                                        ));
                                }
                            }

                            concerts.Add(current);
                        }
                    }
                    return concerts;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<ConcertGenre>? ConcertGenres(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, ConcertGenre? insertConcertGenre = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<ConcertGenre> concertGenres = new();
                    sqlString = "SELECT * FROM concerts.tblConcertGenres " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            concertGenres.Add(new ConcertGenre(
                                reader.GetInt32(reader.GetOrdinal("concertId")),
                                reader.GetInt32(reader.GetOrdinal("genreId"))));
                        }
                    }
                    return concertGenres;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO concerts.tblConcertGenres ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertConcertGenre?.ConcertId.ToString(), "concertId", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertConcertGenre?.GenreId.ToString(), "genreId", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM concerts.tblConcertGenres " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE concerts.tblConcertGenres ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertConcertGenre?.ConcertId.ToString(), "concertId", ref setClause, ref notNull);
                    AddUpdateProperties(insertConcertGenre?.GenreId.ToString(), "genreId", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<Genre>? Genres(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, Genre? insertGenre = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<Genre> genres = new();
                    sqlString = "SELECT * FROM concerts.tblGenres " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            genres.Add(new Genre(
                                reader.GetInt32(reader.GetOrdinal("genreId")),
                                reader.GetString(reader.GetOrdinal("genreName")),
                                reader.GetString(reader.GetOrdinal("genreDescription"))));
                        }
                    }
                    return genres;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO concerts.tblGenres ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertGenre?.GenreName, "genreName", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertGenre?.GenreDescription, "genreDescription", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM concerts.tblGenres " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE concerts.tblGenres ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertGenre?.GenreName, "genreName", ref setClause, ref notNull);
                    AddUpdateProperties(insertGenre?.GenreDescription, "genreDescription", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<Location>? Locations(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, Location? insertLocation = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<Location> locations = new();
                    sqlString = "SELECT * FROM concerts.tblLocations " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            locations.Add(new Location(
                                reader.GetInt32(reader.GetOrdinal("locationId")),
                                reader.GetString(reader.GetOrdinal("locationName")),
                                reader.GetInt32(reader.GetOrdinal("cityId")),
                                reader.GetString(reader.GetOrdinal("locationAddress")),
                                reader.GetInt32(reader.GetOrdinal("locationCapacity"))));
                        }
                    }
                    return locations;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO concerts.tblLocations ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertLocation?.LocationName, "locationName", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertLocation?.CityId.ToString(), "cityId", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertLocation?.LocationAddress, "locationAddress", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertLocation?.LocationCapacity.ToString(), "locationCapacity", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM concerts.tblLocations " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE concerts.tblLocations ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertLocation?.LocationName, "locationName", ref setClause, ref notNull);
                    AddUpdateProperties(insertLocation?.CityId.ToString(), "cityId", ref setClause, ref notNull);
                    AddUpdateProperties(insertLocation?.LocationAddress, "locationAddress", ref setClause, ref notNull);
                    AddUpdateProperties(insertLocation?.LocationCapacity.ToString(), "locationCapacity", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<FullLocation>? FullLocations(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<FullLocation> fullLocations = new();
                    sqlString = "SELECT * FROM concerts.tblLocations " + whereClause + ";";

                    Dictionary<int, string> cities = Cities(SQLAction.Select)?.ToDictionary(city => city.CityId, city => city.CityName) ?? new();

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            FullLocation current = new FullLocation(
                                reader.GetInt32(reader.GetOrdinal("locationId")),
                                reader.GetString(reader.GetOrdinal("locationName")),
                                cities[reader.GetInt32(reader.GetOrdinal("cityId"))],
                                reader.GetString(reader.GetOrdinal("locationAddress")),
                                reader.GetInt32(reader.GetOrdinal("locationCapacity")));

                            fullLocations.Add(current);
                        }
                    }
                    return fullLocations;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<Sale>? Sales(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null, Sale? insertSale = null)
        {
            string sqlString;

            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<Sale> sales = new();
                    sqlString = "SELECT * FROM sales.tblSales " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sales.Add(new Sale(
                                reader.GetInt32(reader.GetOrdinal("saleId")),
                                reader.GetInt32(reader.GetOrdinal("customerId")),
                                reader.GetInt32(reader.GetOrdinal("concertId")),
                                reader.GetInt32(reader.GetOrdinal("saleQuantity"))));
                        }
                    }
                    return sales;

                case SQLAction.Insert:
                    sqlString = "INSERT INTO sales.tblSales ";
                    string values = "VALUES (";
                    string columns = "(";
                    bool notNull = false;

                    AddInsertProperties(insertSale?.CustomerId.ToString(), "customerId", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertSale?.ConcertId.ToString(), "concertId", ref values, ref columns, ref notNull);
                    AddInsertProperties(insertSale?.SaleQuantity.ToString(), "saleQuantity", ref values, ref columns, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += columns + ") " + values + ");";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Delete:
                    sqlString = "DELETE FROM sales.tblSales " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                    return null;

                case SQLAction.Update:
                    sqlString = "UPDATE sales.tblSales ";
                    string setClause = "SET ";
                    notNull = false;

                    AddUpdateProperties(insertSale?.CustomerId.ToString(), "customerId", ref setClause, ref notNull);
                    AddUpdateProperties(insertSale?.ConcertId.ToString(), "concertId", ref setClause, ref notNull);
                    AddUpdateProperties(insertSale?.SaleQuantity.ToString(), "saleQuantity", ref setClause, ref notNull);

                    if (!notNull)
                    {
                        throw new Exception("At least one field must be provided for insertion.");
                    }

                    sqlString += setClause + " " + whereClause + ";";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    return null;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }

        public List<FullSale>? FullSales(SQLAction SQLAction, string whereClause = "", Dictionary<string, object>? parameters = null)
        {
            switch (SQLAction)
            {
                case SQLAction.Select:
                    List<FullSale> sales = new();
                    string sqlString = "SELECT * FROM ( " +
                        "SELECT s.saleId, cs.customerId, cs.customerFirstName, cs.customerLastName, cs.customerPhone, cs.customerEmail, cs.customerUsername, cs.customerPassword, cs.customerIsAdmin, STRING_AGG(ca.addressId, ':') AS addressIds, STRING_AGG(ca.streetAddress, ':') AS streetAddresses, STRING_AGG(ct1.cityName, ':') AS customerCityNames, STRING_AGG(ca.postalCode, ':') AS postalCodes, c.concertId, c.concertName, c.concertDescription, c.concertDate, c.concertTime, c.concertAvailTickets, c.concertTicketPrice, l.locationId, l.locationName, l.locationAddress, l.locationCapacity, ct2.cityId, ct2.cityName, STRING_AGG(g.genreId, ':') AS genreIds, STRING_AGG(g.genreName, ':') AS genreNames, STRING_AGG(g.genreDescription, ':') AS genreDescriptions, s.saleQuantity " +
                        "FROM sales.tblSales AS s INNER JOIN sales.tblCustomers AS cs ON s.customerId = cs.customerId LEFT JOIN sales.tblCustomerAddresses AS ca ON cs.customerId = ca.customerId LEFT JOIN concerts.tblCities AS ct1 ON ca.cityId = ct1.cityId INNER JOIN concerts.tblConcerts AS c ON s.concertId = c.concertId LEFT JOIN concerts.tblConcertGenres AS cg ON c.concertId = cg.concertId LEFT JOIN concerts.tblGenres AS g ON cg.genreId = g.genreId LEFT JOIN concerts.tblLocations AS l ON c.locationId = l.locationId LEFT JOIN concerts.tblCities AS ct2 ON l.cityId = ct2.cityId " +
                        "GROUP BY s.saleId, cs.customerId, cs.customerFirstName, cs.customerLastName, cs.customerPhone, cs.customerEmail, cs.customerUsername, cs.customerPassword, cs.customerIsAdmin, c.concertId, c.concertName, c.concertDescription, c.concertDate, c.concertTime, c.concertAvailTickets, c.concertTicketPrice, l.locationId, l.locationName, l.locationAddress, l.locationCapacity, ct2.cityId, ct2.cityName, s.saleQuantity " +
                        ") AS FullSales " + whereClause + " ;";

                    using (SqlCommand cmd = new(sqlString, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var kvp in parameters)
                            {
                                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                            }
                        }

                        using SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            // Get customer from ordinals
                            int customerId = reader.GetInt32(reader.GetOrdinal("customerId"));

                            FullCustomer customer = new FullCustomer(
                                customerId,
                                reader.GetString(reader.GetOrdinal("customerFirstName")),
                                reader.GetString(reader.GetOrdinal("customerLastName")),
                                reader.GetString(reader.GetOrdinal("customerPhone")),
                                reader.GetString(reader.GetOrdinal("customerEmail")),
                                reader.GetString(reader.GetOrdinal("customerUsername")),
                                reader.GetString(reader.GetOrdinal("customerPassword")),
                                reader.GetBoolean(reader.GetOrdinal("customerIsAdmin")));

                            string[] streetAddresses = reader.IsDBNull(reader.GetOrdinal("streetAddresses"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("streetAddresses")).Split(':').Distinct().ToArray();

                            string[] cityNames = reader.IsDBNull(reader.GetOrdinal("customerCityNames"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("customerCityNames")).Split(':').Distinct().ToArray();

                            string[] postalCodes = reader.IsDBNull(reader.GetOrdinal("postalCodes"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("postalCodes")).Split(':').Distinct().ToArray();

                            string[] addressIds = reader.IsDBNull(reader.GetOrdinal("addressIds"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("addressIds")).Split(':').Distinct().ToArray();

                            if (streetAddresses.Length != cityNames.Length && streetAddresses.Length != postalCodes.Length)
                            {
                                throw new Exception("Mismatching address data");
                            }

                            List<FullCustomerAddress> addresses = new();

                            for (int i = 0; i < streetAddresses.Length; i++)
                            {
                                addresses.Add(new FullCustomerAddress(
                                    int.Parse(addressIds[i]),
                                    customerId,
                                    streetAddresses[i],
                                    cityNames[i],
                                    postalCodes[i]
                                    ));
                            }

                            customer.CustomerAddresses = addresses;

                            // Get concert from ordinals
                            FullConcert concert = new FullConcert(
                                reader.GetInt32(reader.GetOrdinal("concertId")),
                                reader.GetString(reader.GetOrdinal("concertName")),
                                reader.GetString(reader.GetOrdinal("concertDescription")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("concertDate"))),
                                TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("concertTime"))),
                                reader.GetInt32(reader.GetOrdinal("concertAvailTickets")),
                                reader.GetDecimal(reader.GetOrdinal("concertTicketPrice")));

                            concert.ConcertLocation = new FullLocation(
                                reader.GetInt32(reader.GetOrdinal("locationId")),
                                reader.GetString(reader.GetOrdinal("locationName")),
                                reader.GetString(reader.GetOrdinal("locationAddress")),
                                reader.GetString(reader.GetOrdinal("cityName")),
                                reader.GetInt32(reader.GetOrdinal("locationCapacity"))
                                );

                            concert.GenreList = new List<Genre>();
                            string[] genreIds = reader.IsDBNull(reader.GetOrdinal("genreIds"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("genreIds")).Split(':');

                            string[] genreNames = reader.IsDBNull(reader.GetOrdinal("genreNames"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("genreNames")).Split(':');

                            string[] genreDescriptions = reader.IsDBNull(reader.GetOrdinal("genreDescriptions"))
                                ? new string[0]
                                : reader.GetString(reader.GetOrdinal("genreDescriptions")).Split(':');

                            if (genreIds?.Length != genreNames?.Length || genreIds?.Length != genreDescriptions?.Length)
                                throw new Exception("Genre data is missing");

                            if (genreIds != null && genreNames != null && genreDescriptions != null)
                            {
                                for (int i = 0; i < genreIds.Length; i++)
                                {
                                    concert.GenreList.Add(new Genre(
                                        int.Parse(genreIds[i]),
                                        genreNames[i],
                                        genreDescriptions[i]
                                        ));
                                }
                            }

                            // Get sale data
                            FullSale current = new FullSale(
                                reader.GetInt32(reader.GetOrdinal("saleId")),
                                customer,
                                concert,
                                reader.GetInt32(reader.GetOrdinal("saleQuantity")));

                            sales.Add(current);
                        }
                    }
                    return sales;

                default:
                    throw new Exception("Invalid SQLAction");
            }
        }
    }
}
