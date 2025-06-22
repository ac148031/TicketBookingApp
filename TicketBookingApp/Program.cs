using TicketBookingApp.Table_Classes;
using static TicketBookingApp.StorageManager;

namespace TicketBookingApp
{
    public class Program
    {
        private static string Username = string.Empty;

        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TicketBookingDatabase;Integrated Security=True;" +
                "Connection Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite; Multi Subnet Failover=False;";

            var storageManager = new StorageManager(connectionString);
            var view = new ConsoleView();
            //storageManager.Setup();
            //return;

            bool loggedIn = false;

            while (true)
            {
                if (!loggedIn)
                {
                    Username = LoginScreen(view, storageManager);
                    loggedIn = true;
                }

                Dictionary<string, int> menuOptions = new()
                {
                    { "View My Profile", 1 },
                    { "View My Bookings", 2 },
                    { "Log Out", 3 }
                };

                int exitCode = view.Menu(menuOptions);

                if (exitCode == 3)
                {
                    Username = String.Empty;
                    loggedIn = false;
                    continue;
                }
            }
        }

        private static string LoginScreen(ConsoleView view, StorageManager storageManager)
        {
            bool loggedIn = false;
            int errorCode = 0;
            string username = string.Empty;
            do
            {
                (username, string password) = view.Login(errorCode);
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    errorCode = 2;
                    continue;
                }
                else if (username == " " && password == " ")
                {
                    RegisterScreen(view, storageManager);
                    continue;
                }
                else
                {
                    List<Customer>? customers = storageManager.Customers(SQLAction.Select, $"WHERE customerUsername = '{username}'");
                    if (customers.Any(customer => PWSecurity.Verify(password, customer.CustomerPassword)))
                    {
                        errorCode = 0;
                        loggedIn = true;
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        errorCode = 1;
                        continue;
                    }
                }
            } while (!loggedIn);
            return username;
        }

        private static void RegisterScreen(ConsoleView view, StorageManager storageManager)
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
                    List<Customer>? customers = storageManager.Customers(SQLAction.Select, $"WHERE customerUsername = '{username}'");
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
                }
            } while (!registered);
        }
    }
}
