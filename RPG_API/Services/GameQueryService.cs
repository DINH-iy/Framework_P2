using Game.Api.Models;
using Game.Domain;
using Microsoft.EntityFrameworkCore;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class GameQueryService
{
    private readonly GameDbContext _context;
    private readonly GameStateService _gameState;

    public GameQueryService(GameDbContext context, GameStateService gameState)
    {
        _context = context;
        _gameState = gameState;
    }

    public async Task<List<GameSummaryResponse>> GetGamesAsync()
    {
        var games = await _context.Games
            .Include(game => game.Player)
            .OrderByDescending(game => game.Id)
            .ToListAsync();

        var summaries = new List<GameSummaryResponse>();

        foreach (var game in games)
        {
            var summary = new GameSummaryResponse
            {
                Id = game.Id,
                PlayerName = game.Player.Name,
                CurrentLocation = game.CurrentLocation,
                Health = game.Player.Health,
                MaxHealth = game.Player.MaxHealth,
                IsFinished = game.IsFinished,
                IsWon = game.IsWon
            };

            summaries.Add(summary);
        }

        return summaries;
    }

    public async Task<GameResponse> GetAsync(int gameId)
    {
        var game = await _gameState.LoadAsync(gameId);
        return _gameState.CreateResponse(game);
    }
}
