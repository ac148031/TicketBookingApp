using TicketBookingApp.Table_Classes;
using static TicketBookingApp.StorageManager;

namespace TicketBookingApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(PWSecurity.Verify("123456789", "$2a$08$9HMyoipTzjOzIPOfOCzPae4h4IAdsvXY8YuHqhsSTuMpKIygQ8MPC"));

            //return;

            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TicketBookingDatabase;Integrated Security=True;" +
                "Connection Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite; Multi Subnet Failover=False;";

            var storageManager = new StorageManager(connectionString);
            var view = new ConsoleView();
            //storageManager.Setup();
            //return;

            bool loggedIn = false;
            int errorCode = 0;
            string username = string.Empty;
            do
            {
                (username, string password) = view.LogInScreen(errorCode);
                if (username == "0" && password == "0")
                {
                    Console.Clear();
                    return;
                }
                else if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
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


        }

    }
}
