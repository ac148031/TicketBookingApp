namespace TicketBookingApp
{
    public class ConsoleView
    {
        private int WindowWidth = Console.WindowWidth;
        private int WindowHeight = Console.WindowHeight;

        public void LogInScreen()
        {
            Console.Clear();
            DrawHeader();
            DrawFooter();
        }

        private void DrawHeader()
        {
            string header = "Ticket Sales Application";
            int horizontalPos = (int)Math.Round((WindowWidth / 2d) - (header.Length / 2d));
            Console.SetCursorPosition(horizontalPos, 0);
            Console.WriteLine(header);
        }

        private void DrawFooter()
        {
            string footer = "Exit - Ctrl + E";
            int horizontalPos = (int)Math.Round((WindowWidth / 2d) - (footer.Length / 2d));
            Console.SetCursorPosition(horizontalPos, WindowHeight - 1);
            Console.WriteLine(footer);
        }
    }
}
