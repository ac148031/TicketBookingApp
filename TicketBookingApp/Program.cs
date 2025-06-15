using static TicketBookingApp.StorageManager;

namespace TicketBookingApp
{
    public class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=TicketBookingDatabase;Integrated Security=True;" +
                "Connection Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite; Multi Subnet Failover=False;";

            var storageManager = new StorageManager(connectionString);
            var view = new ConsoleView();

            //(string username, string password) = view.LogInScreen(0);
            //if (username == "0" && password == "0") return;

            storageManager.Setup();

            var h = storageManager.Cities(SQLAction.Select);
            foreach (City city in h)
            {
                Console.WriteLine($"City: {city.CityName}, ID: {city.CityId}");
            }
        }

    }
}
