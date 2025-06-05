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

            view.LogInScreen();
        }
    }
}
