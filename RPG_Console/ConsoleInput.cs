namespace RPG_Console;

public static class ConsoleInput
{
    public static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("This value is required.");
        }
    }

    public static int ReadOption(int maximum)
    {
        while (true)
        {
            var input = ReadRequired("> ");
            if (int.TryParse(input, out var option) && option >= 1 && option <= maximum)
                return option;

            Console.WriteLine($"Choose a number from 1 to {maximum}.");
        }
    }

    public static int ReadNumber(string prompt)
    {
        while (true)
        {
            var input = ReadRequired(prompt);
            if (int.TryParse(input, out var number))
                return number;

            Console.WriteLine("Enter a valid number.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
