using System.Reflection.PortableExecutable;
using System.Text;

namespace TicketBookingApp
{
    public class ConsoleView
    {
        private int WindowWidth = Console.WindowWidth;
        private int WindowHeight = Console.WindowHeight;

        public (string username, string password) LogInScreen(int errorCode)
        {
            Console.Clear();
            DrawHeader();
            DrawFooter();

            //Draw login boxes 
            string[] inputFields = [
            "         ┌────────────────────┐",
            "Username │                    │",
            "         ├────────────────────┤",
            "Password │                    │",
            "         └────────────────────┘"];

            int startXPos = (int)Math.Round((WindowWidth / 2d) - (inputFields[0].Length / 2d));
            int startYPos = (int)Math.Round((WindowHeight / 2d) - (5 / 2d)); // Positions for center of console

            for (int i = 0; i < inputFields.Length; i++)
            {
                Console.SetCursorPosition(startXPos, startYPos + i);
                Console.Write(inputFields[i]);
            }

            Dictionary<int, string> errorMessages = new()
            {
                { 1, "Invalid Username or Password" },
                { 2, "Username or Password cannot be empty" }
            };

            if (errorCode != 0)
            {
                int errorXPos = (int)Math.Round((WindowWidth / 2d) - (errorMessages[errorCode].Length / 2d));
                Console.SetCursorPosition(errorXPos, startYPos + inputFields.Length + 1);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(errorMessages[errorCode]);
                Console.ResetColor();
            }

            // Get password and username
            StringBuilder username = new();
            StringBuilder password = new();

            bool inputPassword = false;
            int yOffset = 0;
            int xOffset = 0;

            ConsoleKeyInfo input;

            while (true)
            {
                if (inputPassword)
                {
                    yOffset = 2; // Offset to move cursor down to password box

                    if (password.Length < 20) xOffset = password.Length;
                    else xOffset = 19; // Make sure cursor does not exceed length of box
                }
                else
                {
                    yOffset = 0;

                    if (username.Length < 19) xOffset = username.Length;
                    else
                    {
                        xOffset = 19; // Make sure cursor does not exceed length of box, and move the current text left to simulate scroll.
                        Console.SetCursorPosition(startXPos + 10, startYPos + yOffset + 1);
                        Console.Write(username.ToString().Substring(username.Length - 19, 19) + " ");
                    }
                }

                // Read single key input
                Console.SetCursorPosition(startXPos + xOffset + 10, startYPos + yOffset + 1);
                input = Console.ReadKey(inputPassword);

                // Enter switches to next box, or submits log in
                if (!inputPassword && input.Key == ConsoleKey.Enter) inputPassword = true;
                else if (input.Key == ConsoleKey.Enter) return (username.ToString(), password.ToString());
                // Tab switches boxes
                else if (input.Key == ConsoleKey.Tab) inputPassword = !inputPassword;
                // Ctrl + E exits with codes 0
                else if (input.Key == ConsoleKey.E && (input.Modifiers & ConsoleModifiers.Control) != 0) return ("0", "0");
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (inputPassword)
                    {
                        if (password.Length > 0) password.Length--;
                    }
                    else
                    {
                        if (username.Length > 0) username.Length--;
                        if (username.Length < 19)
                        {
                            Console.SetCursorPosition(startXPos + 10, startYPos + yOffset + 1);
                            Console.Write(username.ToString().Substring(0, username.Length) + " ");
                        }
                    }
                }
                else
                {
                    char c = input.KeyChar;

                    // Only accepts char if is a-Z, a symbol or punctuation, but not space.
                    if (char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                    {
                        if (inputPassword)
                        {
                            password.Append(c);
                        }
                        else
                        {
                            username.Append(c);
                        }
                    }
                }
            }
        }

        private void DrawHeader()
        {
            string line = new string('─', WindowWidth);
            Console.SetCursorPosition(0, 1);
            Console.Write(line);

            string header = "Ticket Sales Application";
            int horizontalPos = (int)Math.Round((WindowWidth / 2d) - (header.Length / 2d));
            Console.SetCursorPosition(horizontalPos, 0);
            Console.Write(header);
        }

        private void DrawFooter()
        {
            string line = new string('─', WindowWidth);
            Console.SetCursorPosition(0, WindowHeight - 2);
            Console.Write(line);

            string footer = "Exit - Ctrl + E";
            int horizontalPos = (int)Math.Round((WindowWidth / 2d) - (footer.Length / 2d));
            Console.SetCursorPosition(horizontalPos, WindowHeight - 1);
            Console.Write(footer);
        }
    }
}
