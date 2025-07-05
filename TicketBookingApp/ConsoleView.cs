using System.Text;
using TicketBookingApp.Table_Classes;

namespace TicketBookingApp
{
    public class ConsoleView
    {
        private readonly int WindowWidth = Console.WindowWidth;
        private readonly int WindowHeight = Console.WindowHeight;

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
                if (!inputPassword && input.MatchesInput("Enter")) inputPassword = true;
                else if (input.MatchesInput("Enter")) return (username.ToString(), password.ToString());
                // Tab switches boxes
                else if (input.MatchesInput("Tab")) inputPassword = !inputPassword;
                else if (input.MatchesInput(["E", "Control"])) Program.Exit();
                else if (input.MatchesInput(["R", "Control"])) return (" ", " ");
                else if (input.MatchesInput("Backspace"))
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

                if (inputBoxSelection != 2 && input.MatchesInput("Enter")) inputBoxSelection++;
                else if (input.MatchesInput("Enter")) return (username.ToString(), password.ToString(), confirmPassword.ToString());
                else if (inputBoxSelection != 2 && input.MatchesInput("Tab")) inputBoxSelection++;
                else if (input.MatchesInput("Tab")) inputBoxSelection = 0;
                else if (input.MatchesInput("UpArrow") && inputBoxSelection > 0) inputBoxSelection--;
                else if (input.MatchesInput("DownArrow") && inputBoxSelection < 2) inputBoxSelection++;
                else if (input.MatchesInput(["E", "Control"])) Program.Exit();
                else if (input.MatchesInput(["B", "Control"])) return (" ", " ", " ");
                else if (input.MatchesInput("Backspace"))
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

        public Customer EditUserDetails(int errorCode)
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

                if (inputBoxSelection < 3 && (input.MatchesInput("Enter") || input.MatchesInput("Tab"))) inputBoxSelection++;
                else if (input.MatchesInput("Enter")) return new(-1, firstName.ToString(), lastName.ToString(), phone.ToString(), email.ToString(), null, null);
                else if (input.MatchesInput("Tab")) inputBoxSelection = 0;
                else if (input.MatchesInput("UpArrow") && inputBoxSelection > 0) inputBoxSelection--;
                else if (input.MatchesInput("DownArrow") && inputBoxSelection < 3) inputBoxSelection++;
                else if (input.MatchesInput(["E", "Control"])) Program.Exit();
                else if (input.MatchesInput(["B", "Control"])) return null;
                else if (input.MatchesInput("Backspace"))
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

                if (input.MatchesInput("UpArrow") && selectedOption > 0) selectedOption--;
                else if (input.MatchesInput("DownArrow") && selectedOption < menuOptionKeys.Length - 1) selectedOption++;
                else if (input.MatchesInput(["E", "Control"]))
                {
                    Program.Exit();
                }
                else if (input.MatchesInput("Tab"))
                {
                    if (selectedOption == menuOptionKeys.Length - 1) selectedOption = 0;
                    else selectedOption++;
                }
                else if (input.MatchesInput("Enter"))
                {
                    Console.CursorVisible = true;
                    return menuOptions[menuOptionKeys[selectedOption]];
                }
            }
        }

        public int ViewUserDetails(StorageManager storageManager, int userId)
        {
            Dictionary<string, object> parameters = new() { { "@id", userId } };
            FullCustomer? user = storageManager.FullCustomers(SQLAction.Select, "WHERE customerId = @id", parameters)?.FirstOrDefault() ?? throw new Exception("Returned null for userId");

            Console.Clear();
            Console.CursorVisible = false;
            DrawHeader($"{user.CustomerUsername}'s Profile");
            DrawFooter(["Edit Details - Ctrl + D", "Back - Ctrl + B"]);

            string[] userDetails = [
                $"First Name: {user.CustomerFirstName}",
                $"Last Name: {user.CustomerLastName}",
                $"Phone: {user.CustomerPhone}",
                $"Email: {user.CustomerEmail}",
                $"Is Admin: {(user.CustomerIsAdmin ? "Yes" : "No")}"
            ];

            int longestOption = userDetails.Aggregate(0, (hold, next) => Math.Max(hold, next.Length));

            int startXPos = (int)Math.Round((WindowWidth / 2d) - (longestOption / 2d));
            int startYPos = (int)Math.Round((WindowHeight / 2d) - (userDetails.Length / 2d));


            for (int i = 0; i < userDetails.Length; i++)
            {
                string detail = userDetails[i];
                Console.SetCursorPosition(startXPos, startYPos + i);
                Console.Write(detail);
            }

            ConsoleKeyInfo input;

            while (true)
            {
                input = Console.ReadKey(true);

                if (input.MatchesInput(["E", "Control"]))
                {
                    Program.Exit();
                }
                else if (input.MatchesInput(["B", "Control"]))
                {
                    return 0;
                }
            }
        }

        public Customer? CustomerSearch(StorageManager storageManager, string initSearch = "", string initPage = "0 0")
        {
            Console.Clear();
            Console.CursorVisible = false;
            DrawHeader("Concerts");
            DrawFooter(["Back - Ctrl + B ", "Detailed View - Enter", "Arrow keys to select"]);

            // Draw searchbox
            string[] searchBox = [
                "┌─Search" + new string('─', WindowWidth - 11) + "┐",
                "│" + initSearch + new string(' ', WindowWidth - 4 - initSearch.Length) + "│",
                "└" + new string('─', WindowWidth - 4) + "┘"
                ];

            for (int i = 0; i < searchBox.Length; i++)
            {
                Console.SetCursorPosition(1, 2 + i);
                Console.Write(searchBox[i]);
            }

            // 4 to account for header + footer, 1 for header
            int paddingAmount = 1;
            int dataDisplayHeight = WindowHeight - searchBox.Length - (paddingAmount * 2) - 4 - 1;
            int dataDisplayWidth = WindowWidth - paddingAmount * 2;

            // Get column widths
            var data = storageManager.Customers(SQLAction.Select);

            List<int> tempColumnSizes =
            [
                data.Max(c => c.CustomerId.ToString().Length),
                data.Max(c => c.CustomerFirstName.Length + c.CustomerLastName.Length),
                data.Max(c => c.CustomerEmail.Length),
                data.Max(c => c.CustomerPhone.Length),
                data.Max(c => c.CustomerUsername.Length)
            ];

            // Make sure it isnt bigger than the width
            while (tempColumnSizes.Sum() > dataDisplayWidth - 4)
            {
                int largestColumnIndex = tempColumnSizes.LastIndexOf(tempColumnSizes.Max());
                tempColumnSizes[largestColumnIndex]--;
            }

            List<int> columnSizes = [tempColumnSizes[0]];
            tempColumnSizes = tempColumnSizes[1..];

            // Make sure it fills the width
            while (tempColumnSizes.Sum() < dataDisplayWidth - 4 - columnSizes[0])
            {
                int largestColumnIndex = tempColumnSizes.IndexOf(tempColumnSizes.Min());
                tempColumnSizes[largestColumnIndex]++;
            }

            columnSizes.AddRange(tempColumnSizes);

            string columnHeadings = "Id".PadRight(columnSizes[0] + 1) +
                                    "Name".PadRight(columnSizes[1] + 1) +
                                    "Email".PadRight(columnSizes[2] + 1) +
                                    "Phone".PadRight(columnSizes[3] + 1) +
                                    "Username".PadRight(columnSizes[4]);

            Console.SetCursorPosition(1, 6);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(columnHeadings);
            Console.ResetColor();

            // Search

            string whereClause = "WHERE customerFirstName LIKE @search + '%' " +
                    "OR customerLastName LIKE @search + '%' " +
                    "OR customerFirstName + ' ' + customerLastName LIKE @search + '%' " +
                    "OR customerEmail LIKE @search + '%' " +
                    "OR CAST( customerId AS VARCHAR) LIKE @search + '%' " +
                    "OR customerUsername LIKE @search + '%'";
            Dictionary<string, object> parameters;
            StringBuilder sb = new(initSearch);
            ConsoleKeyInfo input;

            int selectedLine = int.Parse(initPage.Split(' ')[0]);
            int selectedPage = int.Parse(initPage.Split(' ')[1]);

            int xOffset;

            while (true)
            {
                int lastLine = dataDisplayHeight + 1;
                int lastPage = 0;

                //Display Data

                parameters = new() { { "@search", sb.ToString() } };
                data = storageManager.Customers(SQLAction.Select, whereClause, parameters);

                for (int i = 0; i < dataDisplayHeight; i++)
                {
                    Console.SetCursorPosition(1, 7 + i);
                    if (i + (selectedPage * dataDisplayHeight) < data.Count)
                    {
                        Customer current = data[i + (selectedPage * dataDisplayHeight)];

                        string[] detailList = [
                            current.CustomerId.ToString(),
                            current.CustomerFirstName + " " + current.CustomerLastName,
                            current.CustomerEmail,
                            current.CustomerPhone,
                            current.CustomerUsername
                            ];

                        string details = "";

                        for (int j = 0; j < columnSizes.Count; j++)
                        {
                            if (j != 0) details += " ";
                            details += detailList[j].PadRight(columnSizes[j])[..columnSizes[j]];
                        }

                        if (i == selectedLine)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }

                        Console.Write(details);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(new string(' ', WindowWidth - 1));
                    }
                }

                // Amount of data records / how many records per page
                // Whichever is lower, the display height or the the number of records on screen minus one (to get the last record)
                lastPage = (int)Math.Floor((double)data.Count / dataDisplayHeight);
                lastLine = Math.Min(dataDisplayHeight, data.Count - (dataDisplayHeight * selectedPage)) - 1;

                // Search Data
                if (sb.Length < WindowWidth - 5)
                {
                    xOffset = sb.Length;
                }
                else
                {
                    xOffset = WindowWidth - 5;
                    Console.SetCursorPosition(2, 3);
                    Console.Write(sb.ToString(sb.Length - xOffset, xOffset) + " ");
                }

                Console.SetCursorPosition(2 + xOffset, 3);
                Console.CursorVisible = true;
                input = Console.ReadKey();
                Console.CursorVisible = false;

                if (input.MatchesInput(["E", "Control"])) Program.Exit();
                else if (input.MatchesInput(["B", "Control"])) return null;
                else if (input.MatchesInput("UpArrow") && selectedLine > 0) selectedLine--;
                else if (input.MatchesInput("UpArrow") && selectedLine == 0) selectedLine = lastLine;
                else if (input.MatchesInput("DownArrow") && selectedLine < lastLine) selectedLine++;
                else if (input.MatchesInput("DownArrow") && selectedLine == lastLine) selectedLine = 0;
                else if (input.MatchesInput("RightArrow"))
                {
                    selectedLine = 0;
                    if (selectedPage < lastPage) selectedPage++;
                    else if (selectedPage == lastPage) selectedPage = 0;
                }
                else if (input.MatchesInput("LeftArrow"))
                {
                    selectedLine = 0;
                    if (selectedPage > 0) selectedPage--;
                    else if (selectedPage == 0) selectedPage = lastPage;
                }
                else if (input.MatchesInput("Enter"))
                {
                    int userId = data[selectedLine + (selectedPage * dataDisplayHeight)].CustomerId;
                    string selectSave = selectedLine.ToString() + " " + selectedPage.ToString();
                    string searchSave = sb.ToString();

                    return new Customer(
                        userId,
                        searchSave,
                        selectSave,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty
                        );
                }
                else if (input.MatchesInput("Backspace"))
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        if (sb.Length < WindowWidth - 4)
                        {
                            Console.SetCursorPosition(2, 3);
                            Console.Write(sb.ToString(0, sb.Length) + " ");
                        }

                        selectedPage = 0;
                        selectedLine = 0;
                    }
                }
                else
                {
                    char c = input.KeyChar;

                    if (char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c) || c == ' ')
                    {
                        sb.Append(c);

                        selectedPage = 0;
                        selectedLine = 0;
                    }
                }
            }
        }

        // Sub Methods
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
            }

            Console.CursorVisible = true;
        }
    }

    public static class ConsoleKeyInfoExtensions
    {
        public static bool MatchesInput(this ConsoleKeyInfo input, string[] desiredComponents)
        {
            if (Enum.TryParse(desiredComponents[0], true, out ConsoleKey key) && input.Key == key)
            {
                if (desiredComponents.Length > 1 &&
                    Enum.TryParse(desiredComponents[1], true, out ConsoleModifiers modifiers))
                {
                    return (input.Modifiers & modifiers) == modifiers;
                }
                return true;
            }
            return false;
        }

        public static bool MatchesInput(this ConsoleKeyInfo input, string desired)
        {
            return Enum.TryParse(desired, true, out ConsoleKey key) && input.Key == key;
        }
    }
}
