using RPG_Console.Models;

namespace RPG_Console;

public static class ConsoleRenderer
{
    public static void ShowMainMenu()
    {
        Console.Clear();
        WriteDungeonTitle();
        Console.WriteLine("              MAIN MENU");
        Console.WriteLine("              ---------");
        Console.WriteLine();
        Console.WriteLine("1. Start a new game");
        Console.WriteLine("2. Continue a game");
        Console.WriteLine("3. Exit");
        Console.WriteLine();
    }

    public static void ShowGames(List<GameSummary> games)
    {
        Console.Clear();
        WriteDungeonTitle();
        Console.WriteLine("              YOUR GAMES");
        Console.WriteLine("              ----------");
        Console.WriteLine();

        for (var index = 0; index < games.Count; index++)
        {
            var game = games[index];
            var status = game.IsFinished ? "Finished" : "In progress";
            Console.WriteLine($"{index + 1}. {game.PlayerName} - {game.CurrentLocation} - {status}");
            Console.WriteLine($"   HP: {game.Health} / {game.MaxHealth}");
        }

        Console.WriteLine();
    }

    public static void RenderGame(GameState game)
    {
        Console.Clear();
        WriteDungeonTitle();
        DrawPlayerPanel(game);
        DrawMessage(game);

        if (game.Enemy is not null)
            DrawCombatPanel(game);
        else
            DrawExplorationPanel();

        Console.WriteLine();
    }

    private static void DrawPlayerPanel(GameState game)
    {
        Console.WriteLine("+------------------------------------------------+");
        Console.WriteLine($"| PLAYER: {game.Player?.Name ?? "Unknown",-38}|");
        Console.WriteLine($"| HP: {game.Player?.Health ?? 0} / {game.Player?.MaxHealth ?? 0,-31}|");
        Console.WriteLine($"| LOCATION: {game.CurrentLocation,-31}|");
        Console.WriteLine("+------------------------------------------------+");
        Console.WriteLine();
    }

    private static void DrawMessage(GameState game)
    {
        if (!string.IsNullOrWhiteSpace(game.Message))
        {
            Console.WriteLine("[ EVENT ]");
            Console.WriteLine(game.Message);
            Console.WriteLine();
        }
    }

    private static void DrawCombatPanel(GameState game)
    {
        Console.WriteLine("[ COMBAT ]");
        ShowEnemyArt(game.Enemy!.Name);
        Console.WriteLine($"Enemy: {game.Enemy.Name}");
        Console.WriteLine($"Enemy HP: {game.Enemy.Health} / {game.Enemy.MaxHealth}");
        WriteHealthBar(game.Enemy.Health, game.Enemy.MaxHealth);

        DrawCombatMenu();
    }

    private static void DrawExplorationPanel()
    {
        Console.WriteLine("[ EXPLORATION ]");
        DrawExplorationMenu();
    }

    private static void DrawCombatMenu()
    {
        Console.WriteLine("1. Light attack");
        Console.WriteLine("2. Heavy attack");
        Console.WriteLine("3. Run");
        Console.WriteLine("4. View status");
        Console.WriteLine("5. Stop game");
    }

    private static void DrawExplorationMenu()
    {
        Console.WriteLine("1. Continue to next level");
        Console.WriteLine("2. View status");
        Console.WriteLine("3. Stop game");
    }

    private static void DrawPanelTitle(string title)
    {
        Console.WriteLine("+-----------------------------------------------+");
        Console.WriteLine($"| {title,-45}|");
        Console.WriteLine("+-----------------------------------------------+");
    }

    public static void ShowStatus(GameState game)
    {
        Console.WriteLine("+-----------------------------------------------+");
        Console.WriteLine("| STATUS                                        |");
        Console.WriteLine("+-----------------------------------------------+");
        Console.WriteLine($"Game id: {game.Id}");
        Console.WriteLine($"Location: {game.CurrentLocation}");
        ConsoleInput.Pause();
    }

    private static void WriteDungeonTitle()
    {
        Console.WriteLine("================================================");
        Console.WriteLine("||              D U N G E O N                 ||");
        Console.WriteLine("================================================");
        Console.WriteLine();
    }

    private static void ShowEnemyArt(string enemyName)
    {
        Console.WriteLine("              .--------.");
        Console.WriteLine("             /  /\\  /\\  \\");
        Console.WriteLine("            |  |  ||  |  |");
        Console.WriteLine("            |  |  ||  |  |");
        Console.WriteLine("             \\  \\__/  /");
        Console.WriteLine($"              {enemyName.ToUpper(),-10} ");
        Console.WriteLine();
    }

    private static void WriteHealthBar(int health, int maxHealth)
    {
        var barSize = 20;
        var filledSize = maxHealth == 0 ? 0 : health * barSize / maxHealth;
        var filled = new string('#', filledSize);
        var empty = new string('-', barSize - filledSize);
        Console.WriteLine($"[{filled}{empty}]");
    }
}
