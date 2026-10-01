namespace RPG_Console.Models;

public class GameTurnResult
{
    public GameTurnResult(GameState game, bool shouldContinue)
    {
        Game = game;
        ShouldContinue = shouldContinue;
    }

    public GameState Game { get; }
    public bool ShouldContinue { get; }
}
