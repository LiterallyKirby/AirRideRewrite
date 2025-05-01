using System;
using System.Collections.Generic;

class Shell
{
    static string inputBuffer = "";
    static int cursorPos = 0;
    static List<string> history = new List<string>();
    static int historyIndex = -1;

    static void Main()
    {
        // Subscribe to the CancelKeyPress event to handle Ctrl+C
        Console.CancelKeyPress += new ConsoleCancelEventHandler(HandleCtrlC);

        Console.WriteLine("Simple C# Shell (Press ESC to quit)");
        Console.Write("> ");

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (HandleKey(key))
                RedrawLine();
        }
    }

    // Handle Ctrl+C by overriding the default behavior
    static void HandleCtrlC(object sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;  // Prevent the application from exiting
        Console.WriteLine(); // Just move to the next line
        Console.Write("> "); // Prompt the user again
    }

    static bool HandleKey(ConsoleKeyInfo key)
    {
        // Ctrl combinations
        if ((key.Modifiers & ConsoleModifiers.Control) != 0)
        {
            switch (key.Key)
            {
                case ConsoleKey.C:
                    Console.WriteLine("^C");
                    inputBuffer = "";
                    cursorPos = 0;
                    Console.Write("> ");
                    return false;

                case ConsoleKey.L:
                    Console.Clear();
                    Console.Write("> " + inputBuffer);
                    return false;
                case ConsoleKey.F:
                    Console.Write("test");
                    return false;

                case ConsoleKey.U:
                    inputBuffer = inputBuffer.Substring(cursorPos);
                    cursorPos = 0;
                    return true;

                case ConsoleKey.K:
                    inputBuffer = inputBuffer.Substring(0, cursorPos);
                    return true;
            }
        }

        // Normal keys
        switch (key.Key)
        {
            case ConsoleKey.Enter:
                ExecuteCommand();
                return false;

            case ConsoleKey.LeftArrow:
                if (cursorPos > 0) cursorPos--;
                return true;

            case ConsoleKey.RightArrow:
                if (cursorPos < inputBuffer.Length) cursorPos++;
                return true;

            case ConsoleKey.Home:
                cursorPos = 0;
                return true;

            case ConsoleKey.End:
                cursorPos = inputBuffer.Length;
                return true;

            case ConsoleKey.Backspace:
                if (cursorPos > 0)
                {
                    inputBuffer = inputBuffer.Remove(cursorPos - 1, 1);
                    cursorPos--;
                    return true;
                }
                return false;

            case ConsoleKey.Delete:
                if (cursorPos < inputBuffer.Length)
                {
                    inputBuffer = inputBuffer.Remove(cursorPos, 1);
                    return true;
                }
                return false;

            case ConsoleKey.UpArrow:
                if (history.Count > 0 && historyIndex < history.Count - 1)
                {
                    historyIndex++;
                    inputBuffer = history[history.Count - 1 - historyIndex];
                    cursorPos = inputBuffer.Length;
                    return true;
                }
                return false;

            case ConsoleKey.DownArrow:
                if (historyIndex > 0)
                {
                    historyIndex--;
                    inputBuffer = history[history.Count - 1 - historyIndex];
                    cursorPos = inputBuffer.Length;
                    return true;
                }
                else if (historyIndex == 0)
                {
                    historyIndex = -1;
                    inputBuffer = "";
                    cursorPos = 0;
                    return true;
                }
                return false;

            case ConsoleKey.Tab:
                // Implement tab completion here
                return true;

            case ConsoleKey.Escape:
                Environment.Exit(0);
                return false;

            default:
                if (!char.IsControl(key.KeyChar))
                {
                    inputBuffer = inputBuffer.Insert(cursorPos, key.KeyChar.ToString());
                    cursorPos++;
                    return true;
                }
                return false;
        }
    }

    static void RedrawLine()
    {
        int currentLine = Console.CursorTop;
        Console.SetCursorPosition(0, currentLine);
        Console.Write(new string(' ', Console.WindowWidth - 1));
        Console.SetCursorPosition(0, currentLine);
        Console.Write("> " + inputBuffer);
        Console.SetCursorPosition(2 + cursorPos, currentLine);
    }

    static void ExecuteCommand()
    {
        Console.WriteLine();
        if (!string.IsNullOrWhiteSpace(inputBuffer))
        {
            history.Add(inputBuffer);
            historyIndex = -1;

            // Process command here
            Console.WriteLine($"Executing: {inputBuffer}");

            // Example commands
            if (inputBuffer == "clear")
                Console.Clear();
        }
        inputBuffer = "";
        cursorPos = 0;
        Console.Write("> ");
    }
}

