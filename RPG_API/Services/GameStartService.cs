using Game.Api.Models;
using Game.Domain;
using Game.Domain.Entities;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class GameStartService
{
    private readonly GameDbContext _context;
    private readonly GameStateService _gameState;

    public GameStartService(GameDbContext context, GameStateService gameState)
    {
        _context = context;
        _gameState = gameState;
    }

    public async Task<GameResponse> StartAsync(StartGameRequest request)
    {
        var player = new Player
        {
            Name = request.PlayerName,
        };

        var game = new GameEntity
        {
            Player = player,
            CurrentLocation = "Entrance"
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var savedGame = await _gameState.LoadAsync(game.Id);
        return _gameState.CreateResponse(savedGame, "Your adventure begins.");
    }
}
