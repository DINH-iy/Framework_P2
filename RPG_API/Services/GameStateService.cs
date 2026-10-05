using Game.Api.Models;
using Game.Domain;
using Microsoft.EntityFrameworkCore;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class GameStateService
{
    private readonly GameDbContext _context;
    private readonly GameResponseMapper _responseMapper;

    public GameStateService(GameDbContext context, GameResponseMapper responseMapper)
    {
        _context = context;
        _responseMapper = responseMapper;
    }

    public async Task<GameEntity> LoadAsync(int gameId)
    {
        var game = await _context.Games
            .Include(item => item.Player)
            .Include(item => item.CurrentEnemy)
            .FirstOrDefaultAsync(item => item.Id == gameId);

        if (game is null)
            throw new KeyNotFoundException($"Game {gameId} was not found.");

        return game;
    }

    public void EnsureActive(GameEntity game)
    {
        if (game.IsFinished)
            throw new InvalidOperationException("This game has already finished.");
    }

    public GameResponse CreateResponse(
        GameEntity game,
        string message = "",
        int damageDealt = 0,
        int damageReceived = 0,
        string? playerAttack = null,
        string? enemyAttack = null)
    {
        var response = _responseMapper.Create(
            game,
            message,
            damageDealt,
            damageReceived,
            playerAttack,
            enemyAttack);

        return response;
    }
}
