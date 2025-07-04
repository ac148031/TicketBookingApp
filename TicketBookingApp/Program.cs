﻿// Get all lines of code
// gitbash -
// cd "OneDrive - Avondale College/School/2025/12TPI/TicketBookingApp" && git ls-files '*.cs' '*.sql' -z | xargs -0 wc -l
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public class Program
    {
        private static string Username = string.Empty;
        private static Customer? currentUser = null;
        private static StorageManager storageManager;
        private static ConsoleView view;

        public static void Exit()
        {
            Console.Clear();
            storageManager.CloseConnection();
            System.Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TicketBookingDatabase;Integrated Security=True;" +
                "Connection Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite; Multi Subnet Failover=False;";

            storageManager = new StorageManager(connectionString);
            view = new ConsoleView();

            //storageManager.Setup();
            //Exit();

            while (true)
            {
                if (currentUser == null)
                {
                    Username = LoginScreen();

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
                        { "Browse Concerts", 2 },
                        { "View All Customers", 4 },
                        { "Log Out", 3 }
                    };
                }
                else
                {
                    menuOptions = new()
                    {
                        { "View My Profile", 1 },
                        { "Browse Concerts", 2 },
                        { "Log Out", 3 }
                    };
                }

                int exitCode = view.Menu(menuOptions);

                switch (exitCode)
                {
                    case 1:
                        view.ViewUserDetails(storageManager, currentUser.CustomerId);
                        break;

                    case 2:

                        break;

                    case 3:
                        Username = String.Empty;
                        currentUser = null;
                        continue;

                    case 4:
                        CustomerSearchScreen();
                        break;
                }
            }
        }

        private static string LoginScreen()
        {
            bool loggedIn = false;
            int errorCode = 0;
            do
            {
                (Username, string password) = view.Login(errorCode);
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(password))
                {
                    errorCode = 2;
                    continue;
                }
                else if (Username == " " && password == " ")
                {
                    RegisterScreen();
                    errorCode = 0;
                    continue;
                }
                else
                {
                    List<Customer>? customers = storageManager.Customers(SQLAction.Select,
                                                                         $"WHERE customerUsername = @Username",
                                                                         new() { { "@Username", Username } });
                    if (customers.Any(customer => PWSecurity.Verify(password, customer.CustomerPassword)))
                    {
                        errorCode = 0;
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
            return Username;
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
            bool edited = false;
            int errorCode = 0;
            Customer newCustomer;

            do
            {
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
                else
                {
                    edited = true;
                }

                string message = existing.CustomerId == -1 ? "Registering User" : "Updating User Information";

                Console.Clear();
                Thread loading = new(() => ConsoleView.LoadingText(message));

                loading.Start();

                // Insert or update logic
                if (existing.CustomerId == -1)
                {
                    newCustomer.CustomerPassword = PWSecurity.Hash(existing.CustomerPassword);
                    newCustomer.CustomerUsername = existing.CustomerUsername;

                    storageManager.Customers(SQLAction.Insert, insertCustomer: newCustomer);
                }
                else
                {
                    storageManager.Customers(SQLAction.Update, $"WHERE customerId = {existing.CustomerId}", insertCustomer: newCustomer);
                }

                Thread.Sleep(500);

                loading.Interrupt();

            } while (!edited);
        }

        private static void CustomerSearchScreen()
        {
            int userId = 0;
            string initSearch = "";
            string initPage = "0 0";
            while (true)
            {
                Customer? idSearchPage = view.CustomerSearch(storageManager, initSearch, initPage);

                if (idSearchPage == null) return;

                userId = idSearchPage.CustomerId;
                initSearch = idSearchPage.CustomerFirstName;
                initPage = idSearchPage.CustomerLastName;

                view.ViewUserDetails(storageManager, userId);
            }
        }
    }
}
