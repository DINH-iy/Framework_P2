using RPG_Console.Api;
using RPG_Console.Models;

namespace RPG_Console;

public class ConsoleGame
{
    private readonly GameApiClient _gameApi;

    public ConsoleGame(GameApiClient gameApi)
    {
        _gameApi = gameApi;
    }

    public async Task RunAsync()
    {
        try
        {
            var running = true;

            while (running)
            {
                ConsoleRenderer.ShowMainMenu();
                var option = ConsoleInput.ReadOption(3);

                switch (option)
                {
                    case 1:
                        await StartNewGameAsync();
                        break;

                    case 2:
                        await ContinueGameAsync();
                        break;

                    case 3:
                        running = false;
                        break;
                }
            }
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine($"Could not reach the game API: {exception.Message}");
            ConsoleInput.Pause();
        }
    }

    private async Task StartNewGameAsync()
    {
        var playerName = ConsoleInput.ReadRequired("Player name: ");
        var game = await _gameApi.StartGameAsync(playerName);
        await RunGameLoopAsync(game);
    }

    private async Task ContinueGameAsync()
    {
        var games = await _gameApi.GetGamesAsync();

        if (games.Count == 0)
        {
            Console.WriteLine("There are no saved games yet.");
            ConsoleInput.Pause();
            return;
        }

        ConsoleRenderer.ShowGames(games);
        var selectedGameNumber = ConsoleInput.ReadOption(games.Count);
        var selectedGame = games[selectedGameNumber - 1];
        var game = await _gameApi.GetGameAsync(selectedGame.Id);
        await RunGameLoopAsync(game);
    }

    private async Task RunGameLoopAsync(GameState game)
    {
        var running = true;

        while (running && !game.IsFinished)
        {
            ConsoleRenderer.RenderGame(game);
            var option = ConsoleInput.ReadOption(6);

            try
            {
                GameTurnResult turnResult;

                if (game.Enemy is not null)
                    turnResult = await HandleCombatOptionAsync(option, game);
                else
                    turnResult = await HandleTravelOptionAsync(option, game);

                game = turnResult.Game;
                running = turnResult.ShouldContinue;
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine($"API error: {exception.Message}");
                ConsoleInput.Pause();
            }
        }

        ConsoleRenderer.RenderGame(game);
        if (game.IsWon)
            Console.WriteLine("You won the game.");
        else
            Console.WriteLine("Game over.");
        ConsoleInput.Pause();
    }

    private async Task<GameTurnResult> HandleTravelOptionAsync(int option, GameState game)
    {
        switch (option)
        {
            case 1:
                ConsoleRenderer.ShowMovementMenu();
                var direction = ReadDirection();
                var movedGame = await _gameApi.MoveAsync(game.Id, direction);
                return new GameTurnResult(movedGame, true);

            case 2:
                ConsoleRenderer.ShowInventory(game);
                return new GameTurnResult(game, true);

            case 3:
                var gameAfterItem = await UseItemAsync(game);
                return new GameTurnResult(gameAfterItem, true);

            case 4:
                var equippedGame = await EquipItemAsync(game);
                return new GameTurnResult(equippedGame, true);

            case 5:
                ShowStatus(game);
                return new GameTurnResult(game, true);

            case 6:
                return new GameTurnResult(game, false);

            default:
                return new GameTurnResult(game, true);
        }
    }

    private async Task<GameTurnResult> HandleCombatOptionAsync(int option, GameState game)
    {
        switch (option)
        {
            case 1:
                var attackedGame = await _gameApi.AttackAsync(game.Id, "Light");
                return new GameTurnResult(attackedGame, true);

            case 2:
                var heavyAttackGame = await _gameApi.AttackAsync(game.Id, "Heavy");
                return new GameTurnResult(heavyAttackGame, true);

            case 3:
                var gameAfterItem = await UseItemAsync(game);
                return new GameTurnResult(gameAfterItem, true);

            case 4:
                var escapedGame = await _gameApi.RunAsync(game.Id);
                return new GameTurnResult(escapedGame, true);

            case 5:
                ShowStatus(game);
                return new GameTurnResult(game, true);

            case 6:
                return new GameTurnResult(game, false);

            default:
                return new GameTurnResult(game, true);
        }
    }

    private async Task<GameState> UseItemAsync(GameState game)
    {
        var slot = SelectInventorySlot(game);
        if (slot == 0)
            return game;

        return await _gameApi.UseItemAsync(game.Id, slot);
    }

    private async Task<GameState> EquipItemAsync(GameState game)
    {
        var equipableItems = await _gameApi.GetEquipableItemsAsync(game.Id);
        ConsoleRenderer.ShowEquipableItems(equipableItems);

        if (equipableItems.Count == 0)
        {
            Console.WriteLine("There are no equipable items.");
            ConsoleInput.Pause();
            return game;
        }

        var selection = ConsoleInput.ReadOption(equipableItems.Count);
        return await _gameApi.EquipItemAsync(game.Id, selection);
    }

    private static int SelectInventorySlot(GameState game)
    {
        ConsoleRenderer.ShowInventory(game);
        if (game.Inventory.Count == 0)
            return 0;

        return ConsoleInput.ReadOption(game.Inventory.Count);
    }

    private static string ReadDirection()
    {
        var option = ConsoleInput.ReadOption(4);

        switch (option)
        {
            case 1:
                return "Up";
            case 2:
                return "Down";
            case 3:
                return "Left";
            case 4:
                return "Right";
            default:
                return "Up";
        }
    }

    private static bool ShowStatus(GameState game)
    {
        ConsoleRenderer.ShowStatus(game);
        return true;
    }

}
