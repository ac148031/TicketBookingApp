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
    }
}
