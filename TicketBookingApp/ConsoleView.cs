using System.Text;
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public class ConsoleView
    {
        private readonly int WindowWidth = Console.WindowWidth;
        private readonly int WindowHeight = Console.WindowHeight;

        public static void Exit()
        {
            Console.Clear();
            System.Environment.Exit(0);
        }

        public (string, string) Login(int errorCode)
        {
            Console.Clear();
            Console.CursorVisible = false;
            DrawHeader();
            DrawFooter(["Register - Ctrl + R"]);

            //Draw login boxes 
            string[] inputFields = [
            "         ┌────────────────────┐",
            "Username │                    │",
            "         ├────────────────────┤",
            "Password │                    │",
            "         └────────────────────┘"];

            int startXPos = (int)Math.Round((WindowWidth / 2d) - (inputFields[0].Length / 2d));
            int startYPos = (int)Math.Round((WindowHeight / 2d) - (inputFields.Length / 2d)); // Positions for center of console

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
            int yOffset, xOffset;

            ConsoleKeyInfo input;
            Console.CursorVisible = true;

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
                else if (input.Key == ConsoleKey.E && (input.Modifiers & ConsoleModifiers.Control) != 0) Exit();
                else if (input.Key == ConsoleKey.R && (input.Modifiers & ConsoleModifiers.Control) != 0) return (" ", " ");
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
        public (string, string, string) Register(int errorCode)
        {
            Console.Clear();
            Console.CursorVisible = false;
            DrawHeader("Register");
            DrawFooter(["Back - Ctrl + B"]);

            string[] inputFields = [
            "                 ┌────────────────────┐",
            "        Username │                    │",
            "                 ├────────────────────┤",
            "        Password │                    │",
            "                 ├────────────────────┤",
            "Confirm Password │                    │",
            "                 └────────────────────┘"];

            int startXPos = (int)Math.Round((WindowWidth / 2d) - (inputFields[0].Length / 2d));
            int startYPos = (int)Math.Round((WindowHeight / 2d) - (inputFields.Length / 2d));

            for (int i = 0; i < inputFields.Length; i++)
            {
                Console.SetCursorPosition(startXPos, startYPos + i);
                Console.Write(inputFields[i]);
            }

            Dictionary<int, string> errorMessages = new()
            {
                { 1, "Username or Password cannot be empty" },
                { 2, "Passwords do not match" },
                { 3, "Username is unavailable" },
                { 4, "Password must:\nBe between 8-20 characters\nContain a number\nContain a special character\nContain a lower case letter\nContain an upper case letter"},
                { 5, "Username must:\nBe between 4-20 characters" }
            };

            if (errorCode != 0)
            {
                string[] errorMessage = errorMessages[errorCode].Split('\n');
                int longestLine = errorMessage.Max(line => line.Length);
                int errorXPos = (int)Math.Round((WindowWidth / 2d) - (longestLine / 2d));

                Console.ForegroundColor = ConsoleColor.Red;
                for (int i = 0; i < errorMessage.Length; i++)
                {
                    Console.SetCursorPosition(errorXPos, startYPos + inputFields.Length + 1 + i);
                    Console.Write(errorMessage[i]);
                }
                Console.ResetColor();
            }

            StringBuilder username = new();
            StringBuilder password = new();
            StringBuilder confirmPassword = new();

            Dictionary<int, StringBuilder> inputFieldsDict = new()
            {
                { 0, username },
                { 1, password },
                { 2, confirmPassword }
            };

            int inputBoxSelection = 0;
            int yOffset = 0;
            int xOffset = 0;
            int xBoxOffset = inputFields[0].IndexOf('─');

            ConsoleKeyInfo input;
            Console.CursorVisible = true;

            while (true)
            {
                yOffset = inputBoxSelection * 2;

                if (inputFieldsDict[inputBoxSelection].Length < 19) xOffset = inputFieldsDict[inputBoxSelection].Length;
                else
                {
                    xOffset = 19; // Make sure cursor does not exceed length of box, and move the current text left to simulate scroll.
                    Console.SetCursorPosition(startXPos + inputFields[0].IndexOf('─'), startYPos + yOffset + 1);
                    Console.Write(inputFieldsDict[inputBoxSelection].ToString().Substring(inputFieldsDict[inputBoxSelection].Length - 19, 19) + " ");
                }

                Console.SetCursorPosition(startXPos + xOffset + xBoxOffset, startYPos + yOffset + 1);
                input = Console.ReadKey();

                if (inputBoxSelection != 2 && input.Key == ConsoleKey.Enter) inputBoxSelection++;
                else if (input.Key == ConsoleKey.Enter) return (username.ToString(), password.ToString(), confirmPassword.ToString());
                else if (inputBoxSelection != 2 && input.Key == ConsoleKey.Tab) inputBoxSelection++;
                else if (input.Key == ConsoleKey.Tab) inputBoxSelection = 0;
                else if (input.Key == ConsoleKey.UpArrow && inputBoxSelection > 0) inputBoxSelection--;
                else if (input.Key == ConsoleKey.DownArrow && inputBoxSelection < 2) inputBoxSelection++;
                else if (input.Key == ConsoleKey.E && (input.Modifiers & ConsoleModifiers.Control) != 0) Exit();
                else if (input.Key == ConsoleKey.B && (input.Modifiers & ConsoleModifiers.Control) != 0) return (" ", " ", " ");
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (inputFieldsDict[inputBoxSelection].Length > 0)
                    {
                        inputFieldsDict[inputBoxSelection].Length--;
                    }
                    if (inputFieldsDict[inputBoxSelection].Length < xBoxOffset + 1)
                    {
                        Console.SetCursorPosition(startXPos + xBoxOffset, startYPos + yOffset + 1);
                        Console.Write(inputFieldsDict[inputBoxSelection].ToString().Substring(0, inputFieldsDict[inputBoxSelection].Length) + " ");
                    }
                }
                else
                {
                    char c = input.KeyChar;

                    // Only accepts char if is a-Z, a symbol or punctuation, but not space.
                    if (char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                    {
                        inputFieldsDict[inputBoxSelection].Append(c);
                    }
                }
            }
        }
        public Customer UserDetails(int errorCode)
        {
            Console.Clear();
            Console.CursorVisible = false;
            DrawHeader("Edit Details");
            DrawFooter(["Back - Ctrl + B"]);

            string[] inputFields = [
            "           ┌────────────────────┐",
            "First Name │                    │",
            "           ├────────────────────┤",
            " Last Name │                    │",
            "           ├────────────────────┤",
            "     Phone │                    │",
            "           ├────────────────────┤",
            "     Email │                    │",
            "           └────────────────────┘"];

            int startXPos = (int)Math.Round((WindowWidth / 2d) - (inputFields[0].Length / 2d));
            int startYPos = (int)Math.Round((WindowHeight / 2d) - (inputFields.Length / 2d));

            for (int i = 0; i < inputFields.Length; i++)
            {
                Console.SetCursorPosition(startXPos, startYPos + i);
                Console.Write(inputFields[i]);
            }

            Dictionary<int, string> errorMessages = new()
            {
                { 1, "Fields cannot be empty" },
                { 2, "First Name field cannot be empty" },
                { 3, "Last Name field cannot be empty" },
                { 4, "Phone field cannot be empty" },
                { 5, "Email field cannot be empty" }
            };

            if (errorCode != 0)
            {
                string[] errorMessage = errorMessages[errorCode].Split('\n');
                int longestLine = errorMessage.Max(line => line.Length);
                int errorXPos = (int)Math.Round((WindowWidth / 2d) - (longestLine / 2d));

                Console.ForegroundColor = ConsoleColor.Red;
                for (int i = 0; i < errorMessage.Length; i++)
                {
                    Console.SetCursorPosition(errorXPos, startYPos + inputFields.Length + 1 + i);
                    Console.Write(errorMessage[i]);
                }
                Console.ResetColor();
            }

            StringBuilder firstName = new();
            StringBuilder lastName = new();
            StringBuilder phone = new();
            StringBuilder email = new();

            Dictionary<int, StringBuilder> inputFieldsDict = new()
            {
                { 0, firstName },
                { 1, lastName },
                { 2, phone },
                { 3, email }
            };

            int inputBoxSelection = 0;
            int yOffset = 0;
            int xOffset = 0;
            int xBoxOffset = inputFields[0].IndexOf('─');

            ConsoleKeyInfo input;
            Console.CursorVisible = true;

            while (true)
            {
                yOffset = inputBoxSelection * 2;

                if (inputFieldsDict[inputBoxSelection].Length < 19) xOffset = inputFieldsDict[inputBoxSelection].Length;
                else
                {
                    xOffset = 19; // Make sure cursor does not exceed length of box, and move the current text left to simulate scroll.
                    Console.SetCursorPosition(startXPos + xBoxOffset, startYPos + yOffset + 1);
                    Console.Write(inputFieldsDict[inputBoxSelection].ToString().Substring(inputFieldsDict[inputBoxSelection].Length - 19, 19) + " ");
                }

                Console.SetCursorPosition(startXPos + xOffset + xBoxOffset, startYPos + yOffset + 1);
                input = Console.ReadKey();

                if (inputBoxSelection < 3 && (input.Key == ConsoleKey.Enter || input.Key == ConsoleKey.Tab)) inputBoxSelection++;
                else if (input.Key == ConsoleKey.Enter) return new(-1, firstName.ToString(), lastName.ToString(), phone.ToString(), email.ToString(), null, null);
                else if (input.Key == ConsoleKey.Tab) inputBoxSelection = 0;
                else if (input.Key == ConsoleKey.UpArrow && inputBoxSelection > 0) inputBoxSelection--;
                else if (input.Key == ConsoleKey.DownArrow && inputBoxSelection < 3) inputBoxSelection++;
                else if (input.Key == ConsoleKey.E && (input.Modifiers & ConsoleModifiers.Control) != 0) Exit();
                else if (input.Key == ConsoleKey.B && (input.Modifiers & ConsoleModifiers.Control) != 0) return null;
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (inputFieldsDict[inputBoxSelection].Length > 0) inputFieldsDict[inputBoxSelection].Length--;
                    if (inputFieldsDict[inputBoxSelection].Length < 19)
                    {
                        Console.SetCursorPosition(startXPos + xBoxOffset, startYPos + yOffset + 1);
                        Console.Write(inputFieldsDict[inputBoxSelection].ToString().Substring(0, inputFieldsDict[inputBoxSelection].Length) + " ");
                    }
                }
                else
                {
                    char c = input.KeyChar;

                    // Only accepts char if is a-Z, a symbol or punctuation, but not space.
                    if (char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                    {
                        inputFieldsDict[inputBoxSelection].Append(c);
                    }
                }
            }
        }

        public int Menu(Dictionary<string, int> menuOptions)
        {
            Console.Clear();
            Console.CursorVisible = false;
            DrawHeader("Main Menu");
            DrawFooter();

            string[] menuOptionKeys = menuOptions.Keys.ToArray();

            //string[] menuOptionKeys = ["View My Profile", "View My Bookings", "option 2", "option 3"];
            int longestOption = menuOptionKeys.Aggregate(0, (hold, next) => Math.Max(hold, next.Length));
            longestOption = longestOption < 23 ? 23 : longestOption;

            int startXPos = (int)Math.Round((WindowWidth / 2d) - ((longestOption + 4) / 2d));
            int startYPos = (int)Math.Round((WindowHeight / 2d) - (menuOptionKeys.Length + 1));

            for (int i = 0; i < (2 * menuOptionKeys.Length) + 1; i++)
            {
                Console.SetCursorPosition(startXPos, startYPos + i);
                if (i % 2 == 0)
                {
                    if (i == 0)
                    {
                        Console.Write("┌" + new string('─', longestOption + 2) + "┐");
                    }
                    else if (i == menuOptionKeys.Length * 2)
                    {
                        Console.Write("└" + new string('─', longestOption + 2) + "┘");
                    }
                    else
                    {
                        Console.Write("├" + new string('─', longestOption + 2) + "┤");
                    }
                }
                else
                {
                    Console.Write("│" + new string(' ', longestOption + 2) + "│");
                }
            }

            int selectedOption = 0;
            ConsoleKeyInfo input;

            while (true)
            {
                for (int i = 0; i < menuOptionKeys.Length; i++)
                {
                    int xPos = (int)Math.Round((WindowWidth / 2d) - (menuOptionKeys[i].Length / 2d));
                    Console.SetCursorPosition(xPos, startYPos + (i * 2) + 1);
                    if (i == selectedOption) Console.ForegroundColor = ConsoleColor.White;
                    else Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(menuOptionKeys[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.UpArrow && selectedOption > 0) selectedOption--;
                else if (input.Key == ConsoleKey.DownArrow && selectedOption < menuOptionKeys.Length - 1) selectedOption++;
                else if (input.Key == ConsoleKey.E && (input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    Console.CursorVisible = true;
                    System.Environment.Exit(0);
                }
                else if (input.Key == ConsoleKey.Tab)
                {
                    if (selectedOption == menuOptionKeys.Length - 1) selectedOption = 0;
                    else selectedOption++;
                }
                else if (input.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    return menuOptions[menuOptionKeys[selectedOption]];
                }
            }
        }

        private void DrawHeader(string? screen = null)
        {
            string line = new('─', WindowWidth);
            Console.SetCursorPosition(0, 1);
            Console.Write(line);

            string header = "Ticket Sales Application";
            if (screen != null)
            {
                header += " - " + screen;
            }

            int horizontalPos = (int)Math.Round((WindowWidth / 2d) - (header.Length / 2d));
            Console.SetCursorPosition(horizontalPos, 0);
            Console.Write(header);
        }

        private void DrawFooter(string[]? footerAdd = null)
        {
            string line = new('─', WindowWidth);
            Console.SetCursorPosition(0, WindowHeight - 2);
            Console.Write(line);

            string footer = "Exit - Ctrl + E";
            if (footerAdd != null)
            {
                foreach (string add in footerAdd)
                {
                    footer += " | " + add;
                }
            }

            int horizontalPos = (int)Math.Round((WindowWidth / 2d) - (footer.Length / 2d));
            Console.SetCursorPosition(horizontalPos, WindowHeight - 1);
            Console.Write(footer);
        }

        public static void LoadingText(string message)
        {
            Console.CursorVisible = false;
            try
            {
                string[] dotSequence = ["   ", ".  ", ".. ", "..."];

                (int left, int top) = Console.GetCursorPosition();

                for (int i = 0; true; i++)
                {
                    if (i >= 4) i = 0;

                    Console.SetCursorPosition(left, top);

                    Console.Write(message + dotSequence[i]);

                    Thread.Sleep(250);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.CursorVisible = true;
                Console.WriteLine();
            }

            Console.CursorVisible = true;
            Console.WriteLine();
        }
    }
}
