// Get all lines of code
// gitbash -
// cd "OneDrive - Avondale College/School/2025/12TPI/TicketBookingApp" && git ls-files '*.cs' '*.sql' -z | xargs -0 wc && cd
// cd "OneDrive - Avondale College/School/2025/12TPI/TicketBookingApp" && git ls-files '*.cs' -z | xargs -0 wc && cd
using System.Text.RegularExpressions;
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public class Program
    {
        private static string Username = string.Empty;
        private static Customer? currentUser = null;

        private static readonly string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TicketBookingDatabase;Integrated Security=True; Connection Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite; Multi Subnet Failover=False;";
        private static readonly StorageManager storageManager = new(connectionString);
        private static readonly ConsoleView view = new(storageManager);

        public static void Exit()
        {
            Console.Clear();
            storageManager.CloseConnection();
            System.Environment.Exit(0);
        }

        static void Main()
        {
            //storageManager.Setup();
            //Exit();

            while (true)
            {
                if (currentUser == null)
                {
                    LoginScreen();

                    List<Customer>? users = storageManager.Customers(SQLAction.Select,
                                                               $"WHERE customerUsername = @Username",
                                                             new() { { "@Username", Username } });

                    if (users != null && users.Count == 1)
                        currentUser = users[0];
                    else
                        throw new Exception("Should not be more than one user to a username");
                }

                Dictionary<string, int> menuOptions;

                if (currentUser.CustomerIsAdmin)
                {
                    menuOptions = new()
                    {
                        { "View My Profile", 1 },
                        { "View All Concerts", 2 },
                        { "View All Customers", 4 },
                        { "View All Sales", 5 },
                        { "View All Locations", 6 },
                        { "Log Out", 3 }
                    };
                }
                else
                {
                    menuOptions = new()
                    {
                        { "View My Profile", 1 },
                        { "Browse Concerts", 2 },
                        { "View All Locations", 6 },
                        { "Log Out", 3 }
                    };
                }

                int exitCode = view.Menu(menuOptions);

                switch (exitCode)
                {
                    case 1:
                        int deleted = ViewProfileScreen();
                        if (deleted == 1)
                        {
                            Username = String.Empty;
                            currentUser = null;
                            continue;
                        }
                        break;

                    case 2:
                        ConcertSearchScreen();
                        break;

                    case 3:
                        Username = String.Empty;
                        currentUser = null;
                        continue;

                    case 4:
                        CustomerSearchScreen();
                        break;

                    case 5:
                        SalesSearchScreen();
                        break;

                    case 6:
                        LocationsSearchScreen();
                        break;
                }
            }
        }

        private static void LoginScreen()
        {
            bool loggedIn = false;
            int errorCode = 0;
            do
            {
                (string tempUsername, string password) = view.Login(errorCode);
                if (string.IsNullOrEmpty(tempUsername) || string.IsNullOrEmpty(password))
                {
                    errorCode = 2;
                    continue;
                }
                else if (tempUsername == " " && password == " ")
                {
                    RegisterScreen();
                    errorCode = 0;
                    continue;
                }
                else
                {
                    List<Customer>? customers = storageManager.Customers(SQLAction.Select,
                                                                         $"WHERE customerUsername = @Username",
                                                                         new() { { "@Username", tempUsername } });
                    if (customers?.Count != 0 && (customers?.All(customer => PWSecurity.Verify(password, customer.CustomerPassword)) ?? false))
                    {
                        errorCode = 0;
                        Username = tempUsername;
                        loggedIn = true;
                        continue;
                    }
                    else
                    {
                        errorCode = 1;
                        continue;
                    }

                }
            } while (!loggedIn);
        }

        private static void RegisterScreen()
        {
            bool registered = false;
            int errorCode = 0;
            string username, password, confirmPassword;
            do
            {
                (username, password, confirmPassword) = view.Register(errorCode);

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    errorCode = 1;
                    continue;
                }
                else if (username == " " && password == " " && confirmPassword == " ")
                {
                    return;
                }
                else if (password != confirmPassword)
                {
                    errorCode = 2;
                    continue;
                }
                else
                {
                    List<Customer>? customers = storageManager.Customers(SQLAction.Select,
                                                                         $"WHERE customerUsername = @Username",
                                                                         new() { { "@Username", username } });
                    if (customers?.Count != 0)
                    {
                        errorCode = 3;
                        continue;
                    }

                    char[] brokenPassword = password.ToCharArray();
                    char[] brokenUsername = username.ToCharArray();
                    bool appropriatePassword = true;
                    bool appropriateUsername = true;

                    if (password.Length < 8 || password.Length > 20) appropriatePassword = false;
                    if (username.Length < 4 || password.Length > 20) appropriateUsername = false;
                    if (!brokenPassword.All(c => char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c)))
                    {
                        appropriatePassword = false;
                    }
                    if (!brokenUsername.All(c => char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c)))
                    {
                        appropriateUsername = false;
                    }
                    if (!brokenPassword.Any(char.IsDigit)) appropriatePassword = false;
                    if (!brokenPassword.Any(c => char.IsSymbol(c) || char.IsPunctuation(c))) appropriatePassword = false;
                    if (!brokenPassword.Any(char.IsLower)) appropriatePassword = false;
                    if (!brokenPassword.Any(char.IsUpper)) appropriatePassword = false;

                    if (!appropriateUsername)
                    {
                        errorCode = 5;
                        continue;
                    }
                    if (!appropriatePassword)
                    {
                        errorCode = 4;
                        continue;
                    }

                    Customer customerLoginDetails = new(-1, null, null, null, null, username, password);
                    EditProfileScreen(customerLoginDetails);
                    registered = true;
                }
            } while (!registered);
        }

        private static void EditProfileScreen(Customer existing)
        {
            int errorCode = 0;
            Customer newCustomer;
            List<CustomerAddress> newAddresses = new();

            while (true)
            {
                if (existing.CustomerId != -1)
                    newCustomer = view.EditUserDetails(errorCode, existing);
                else
                    newCustomer = view.EditUserDetails(errorCode);

                if (newCustomer == null) return;

                // User input handling logic
                int emptyValues = 0;
                int emptyProperty = -1;
                if (string.IsNullOrEmpty(newCustomer.CustomerFirstName))
                {
                    emptyProperty = 0;
                    emptyValues++;
                }
                if (string.IsNullOrEmpty(newCustomer.CustomerLastName))
                {
                    emptyProperty = 1;
                    emptyValues++;
                }
                if (string.IsNullOrEmpty(newCustomer.CustomerPhone))
                {
                    emptyProperty = 2;
                    emptyValues++;
                }
                if (string.IsNullOrEmpty(newCustomer.CustomerEmail))
                {
                    emptyProperty = 3;
                    emptyValues++;
                }

                if (emptyValues > 1)
                {
                    errorCode = 1;
                    continue;
                }
                else if (emptyValues == 1)
                {
                    errorCode = 2 + emptyProperty;
                    continue;
                }
                else if (!Regex.IsMatch(newCustomer.CustomerPhone.Replace(" ", ""), @"^\+?\d{7,15}$"))
                {
                    errorCode = 6;
                    continue;
                }
                else if (!newCustomer.CustomerEmail.Contains('@'))
                {
                    errorCode = 7;
                    continue;
                }

                break;
            }

            if (existing.CustomerId != -1)
            {
                List<CustomerAddress>? existingAddresses = storageManager.CustomerAddresses(SQLAction.Select, "WHERE customerId = @id", new() { { "@id", existing.CustomerId } });

                int repeats = existingAddresses != null ? Math.Max(existingAddresses.Count + 1, 1) : 1;
                errorCode = 0;

                for (int i = 0; i < repeats; i++)
                {
                    CustomerAddress? current;
                    if (i < existingAddresses?.Count) current = existingAddresses[i];
                    else current = null;

                    while (true)
                    {
                        CustomerAddress? newAddress = view.EditCustomerAddress(errorCode, current);

                        if (newAddress == null) return;
                        if (newAddress.AddressId == -2) break;

                        int emptyValues = 0;
                        int emptyProperty = -1;
                        if (string.IsNullOrEmpty(newAddress.StreetAddress))
                        {
                            emptyProperty = 0;
                            emptyValues++;
                        }
                        if (newAddress.CityId == -2)
                        {
                            emptyProperty = 1;
                            emptyValues++;
                        }
                        if (string.IsNullOrEmpty(newAddress.PostalCode))
                        {
                            emptyProperty = 2;
                            emptyValues++;
                        }

                        if (emptyValues > 1)
                        {
                            errorCode = 1;
                            continue;
                        }
                        else if (emptyValues == 1)
                        {
                            errorCode = 2 + emptyProperty;
                            continue;
                        }
                        else if (newAddress.CityId == -1)
                        {
                            errorCode = 5;
                            continue;
                        }
                        else if (!Regex.IsMatch(newAddress.PostalCode, @"^\d{4}$"))
                        {
                            errorCode = 6;
                            continue;
                        }

                        if (current != null) newAddress.AddressId = current.AddressId;

                        newAddresses.Add(newAddress);
                        break;
                    }
                }
            }

            string message = existing.CustomerId == -1 ? "Registering User" : "Updating User Information";

            Console.Clear();
            Thread loading = new(() => ConsoleView.LoadingText(message));

            loading.Start();

            int customerId;

            // Insert or update logic
            if (existing.CustomerId == -1)
            {
                newCustomer.CustomerPassword = PWSecurity.Hash(existing.CustomerPassword);
                newCustomer.CustomerUsername = existing.CustomerUsername;

                storageManager.Customers(SQLAction.Insert, insertCustomer: newCustomer);

                customerId = storageManager.Customers(SQLAction.Select, "WHERE customerUsername = @username", new Dictionary<string, object> { { "@username", existing.CustomerUsername } })?.FirstOrDefault()?.CustomerId ?? throw new Exception("Unable to fetch customer ID");
            }
            else
            {
                storageManager.Customers(SQLAction.Update, $"WHERE customerId = {existing.CustomerId}", insertCustomer: newCustomer);
                customerId = existing.CustomerId;
            }

            for (int i = 0; i < newAddresses.Count; i++)
            {
                newAddresses[i].CustomerId = customerId;

                if (newAddresses[i].AddressId == -1)
                    storageManager.CustomerAddresses(SQLAction.Insert, insertCustomerAddress: newAddresses[i]);
                else
                    storageManager.CustomerAddresses(SQLAction.Update, $"WHERE addressId = {newAddresses[i].AddressId}", insertCustomerAddress: newAddresses[i]);
            }

            Thread.Sleep(500);

            loading.Interrupt();
        }

        private static int ViewProfileScreen()
        {
            if (currentUser == null) throw new Exception("Cannot run method with null customer");

            Dictionary<string, int> menuOptions = new()
            {
                { "Edit Profile", 1 },
                { "Delete Profile", 2 }
            };

            while (true)
            {
                int exitCode = view.ViewUserDetails(currentUser.CustomerId, menuOptions);

                if (exitCode == 0) return 0;
                else if (exitCode == 1)
                {
                    EditProfileScreen(currentUser);
                    currentUser = storageManager.Customers(SQLAction.Select,
                                                               $"WHERE customerUsername = @Username",
                                                             new() { { "@Username", Username } })?.FirstOrDefault() ?? throw new Exception("Null customer returned");
                }
                else if (exitCode == 2)
                {
                    return DeleteConfirmationScreen<Customer>(currentUser.CustomerId);
                }
            }
        }

        private static int DeleteConfirmationScreen<T>(int id)
        {
            int exitCode = 0;

            if (typeof(T) == typeof(Customer))
            {
                Customer user = storageManager.Customers(SQLAction.Select, $"WHERE customerId = {id}")?.FirstOrDefault() ?? throw new Exception("Customer Id returned Null");
                exitCode = view.DeleteConfirmation(user.CustomerUsername);
            }
            else if (typeof(T) == typeof(Concert))
            {
                Concert concert = storageManager.Concerts(SQLAction.Select, $"WHERE concertId = {id}")?.FirstOrDefault() ?? throw new Exception("Concert Id returned Null");
                exitCode = view.DeleteConfirmation(concert.ConcertName);
            }
            else if (typeof(T) == typeof(City))
            {
                City city = storageManager.Cities(SQLAction.Select, $"WHERE cityId = {id}")?.FirstOrDefault() ?? throw new Exception("City Id returned Null");
                exitCode = view.DeleteConfirmation(city.CityName);
            }
            else if (typeof(T) == typeof(Location))
            {
                Location location = storageManager.Locations(SQLAction.Select, $"WHERE locationId = {id}")?.FirstOrDefault() ?? throw new Exception("Location Id returned Null");
                exitCode = view.DeleteConfirmation(location.LocationName);
            }
            else if (typeof(T) == typeof(CustomerAddress))
            {
                CustomerAddress address = storageManager.CustomerAddresses(SQLAction.Select, $"WHERE addressId = {id}")?.FirstOrDefault() ?? throw new Exception("Address Id returned Null");
                exitCode = view.DeleteConfirmation($"{address.StreetAddress}");
            }
            else if (typeof(T) == typeof(Sale))
            {
                Sale sale = storageManager.Sales(SQLAction.Select, $"WHERE saleId = {id}")?.FirstOrDefault() ?? throw new Exception("Sale Id returned Null");
                exitCode = view.DeleteConfirmation($"{sale.SaleId}");
            }
            else if (typeof(T) == typeof(ConcertGenre))
            {
                ConcertGenre concertGenre = storageManager.ConcertGenres(SQLAction.Select, $"WHERE concertId = {id}")?.FirstOrDefault() ?? throw new Exception("ConcertGenre Id returned Null");
                exitCode = view.DeleteConfirmation($"{concertGenre.ConcertId}-{concertGenre.GenreId}");
            }
            else if (typeof(T) == typeof(Genre))
            {
                Genre genre = storageManager.Genres(SQLAction.Select, $"WHERE genreId = {id}")?.FirstOrDefault() ?? throw new Exception("Genre Id returned Null");
                exitCode = view.DeleteConfirmation(genre.GenreName);
            }

            if (exitCode == 1)
            {
                if (typeof(T) == typeof(Customer))
                {
                    storageManager.Customers(SQLAction.Delete, $"WHERE customerId = {id}");
                }
                else if (typeof(T) == typeof(Concert))
                {
                    storageManager.Concerts(SQLAction.Delete, $"WHERE concertId = {id}");
                }
                else if (typeof(T) == typeof(City))
                {
                    storageManager.Cities(SQLAction.Delete, $"WHERE cityId = {id}");
                }
                else if (typeof(T) == typeof(Location))
                {
                    storageManager.Locations(SQLAction.Delete, $"WHERE locationId = {id}");
                }
                else if (typeof(T) == typeof(CustomerAddress))
                {
                    storageManager.CustomerAddresses(SQLAction.Delete, $"WHERE addressId = {id}");
                }
                else if (typeof(T) == typeof(Sale))
                {
                    storageManager.Sales(SQLAction.Delete, $"WHERE saleId = {id}");
                }
                else if (typeof(T) == typeof(ConcertGenre))
                {
                    storageManager.ConcertGenres(SQLAction.Delete, $"WHERE concertId = {id}");
                }
                else if (typeof(T) == typeof(Genre))
                {
                    storageManager.Genres(SQLAction.Delete, $"WHERE genreId = {id}");
                }
            }

            return exitCode;
        }

        private static void ConcertSearchScreen()
        {
            if (currentUser == null) throw new Exception("Cannot run this method with null customer");

            int concertId = 0;
            string initSearch = "";
            string initPage = "0 0";
            while (true)
            {
                Concert? idSearchPage = view.ConcertSearch(initSearch, initPage);

                if (idSearchPage == null) return;

                concertId = idSearchPage.ConcertId;
                initSearch = idSearchPage.ConcertName;
                initPage = idSearchPage.ConcertDescription;

                Dictionary<string, int> menuOptions;
                if (currentUser.CustomerIsAdmin)
                {
                    menuOptions = new()
                    {
                        { "Buy ticket", 1 },
                        { "Edit Concert", 2 },
                        { "Delete Concert", 3 }
                    };
                }
                else
                {
                    menuOptions = new()
                    {
                        { "Buy ticket", 1 }
                    };
                }

                while (true)
                {
                    int exitCode = view.ViewConcertDetails(concertId, menuOptions);

                    if (exitCode == 0) break;
                    else if (exitCode == 1) BuyTicketScreen(concertId);
                    else if (exitCode == 2) ;
                    else if (exitCode == 3) DeleteConfirmationScreen<Concert>(concertId);
                }
            }
        }

        private static void CustomerSearchScreen()
        {
            int userId = 0;
            string initSearch = "";
            string initPage = "0 0";

            Dictionary<string, int> menuOptions = new()
            {
                { "Edit Customer", 1 },
                { "Delete Customer", 2 }
            };

            while (true)
            {
                Customer? idSearchPage = view.CustomerSearch(initSearch, initPage);

                if (idSearchPage == null) return;

                userId = idSearchPage.CustomerId;
                initSearch = idSearchPage.CustomerFirstName;
                initPage = idSearchPage.CustomerLastName;

                view.ViewUserDetails(userId, menuOptions);
            }
        }

        private static void LocationsSearchScreen()
        {
            int userId = 0;
            string initSearch = "";
            string initPage = "0 0";

            Dictionary<string, int> menuOptions = new()
            {
                { "", 1 },
                { "", 2 }
            };

            while (true)
            {
                Location? idSearchPage = view.LocationSearch(initSearch, initPage);

                if (idSearchPage == null) return;

                userId = idSearchPage.LocationId;
                initSearch = idSearchPage.LocationName;
                initPage = idSearchPage.LocationAddress;

                // View location details
                //view.ViewUserDetails(storageManager, userId, menuOptions);
            }
        }

        private static void SalesSearchScreen()
        {
            view.SalesSearch();
        }

        private static void BuyTicketScreen(int concertId)
        {
            if (currentUser == null) throw new Exception("Cannot run this method with null customer");

            int ticketAmount = view.CreateSale(concertId, currentUser.CustomerUsername);

            if (ticketAmount == 0) return;

            Sale insert = new(-1, currentUser.CustomerId, concertId, ticketAmount);

            storageManager.Sales(SQLAction.Insert, insertSale: insert);
        }
    }
}
