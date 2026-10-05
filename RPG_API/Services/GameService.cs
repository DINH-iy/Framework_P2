using Game.Api.Models;

namespace Game.Api.Services;

public class GameService
{
    private readonly GameStartService _gameStart;
    private readonly GameQueryService _gameQuery;
    private readonly MovementService _movement;
    private readonly CombatGameService _combat;

    public GameService(GameStartService gameStart, GameQueryService gameQuery, MovementService movement, CombatGameService combat)
    {
        _gameStart = gameStart;
        _gameQuery = gameQuery;
        _movement = movement;
        _combat = combat;
    }

    public async Task<GameResponse> StartAsync(StartGameRequest request)
    {
        var game = await _gameStart.StartAsync(request);
        return game;
    }

    public async Task<List<GameSummaryResponse>> GetGamesAsync()
    {
        var games = await _gameQuery.GetGamesAsync();
        return games;
    }

    public async Task<GameResponse> GetAsync(int gameId)
    {
        var game = await _gameQuery.GetAsync(gameId);
        return game;
    }

    public async Task<GameResponse> MoveAsync(int gameId)
    {
        var game = await _movement.MoveAsync(gameId);
        return game;
    }

    public async Task<GameResponse> AttackAsync(int gameId, AttackType attackType)
    {
        var game = await _combat.AttackAsync(gameId, attackType);
        return game;
    }

    public async Task<GameResponse> RunAsync(int gameId)
    {
        var game = await _combat.RunAsync(gameId);
        return game;
    }
}
