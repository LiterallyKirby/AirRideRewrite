using System;

class MainClass
{
    static string InputLine = "";
    static int CursorPosition = 0;

    static void Main()
    {
        Console.WriteLine("Press keys. Press Enter to submit. ESC to quit.");

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
                break;

            CheckInput(key);
            RedrawInput();
        }
    }

    static void CheckInput(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
                Console.WriteLine("[Left Arrow]");
                CursorPosition--;
                break;
            case ConsoleKey.RightArrow:
                CursorPosition++;
                break;

            case ConsoleKey.Enter:
                Console.WriteLine($"\nYou typed: {InputLine}");
                InputLine = "";
                break;

            default:
                InputLine += keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);
                break;
        }
    }

    static void RedrawInput()
    {
        // Move the cursor back to the start of the line
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write("> " + InputLine);
        Console.SetCursorPosition(InputLine.Length + 2, Console.CursorTop); // 2 for "> "
    }
}

