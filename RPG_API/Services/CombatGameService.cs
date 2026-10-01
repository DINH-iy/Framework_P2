using Game.Api.Models;
using Game.Domain;

namespace Game.Api.Services;

public class CombatGameService
{
    private readonly GameDbContext _context;
    private readonly GameStateService _gameState;
    private readonly CombatRoundService _combatRound;

    public CombatGameService(
        GameDbContext context,
        GameStateService gameState,
        CombatRoundService combatRound)
    {
        _context = context;
        _gameState = gameState;
        _combatRound = combatRound;
    }

    public async Task<GameResponse> AttackAsync(int gameId, AttackType playerAttackType)
    {
        var game = await _gameState.LoadAsync(gameId);
        _gameState.EnsureActive(game);

        if (game.CurrentEnemy is null)
            throw new InvalidOperationException("There is no enemy to attack.");

        var round = _combatRound.Resolve(game, playerAttackType);
        await _context.SaveChangesAsync();

        var response = _gameState.CreateResponse(
            game,
            round.Message,
            round.DamageDealt,
            round.DamageReceived,
            round.PlayerAttack.Type.ToString(),
            round.EnemyAttack.Type.ToString());

        return response;
    }

    public async Task<GameResponse> RunAsync(int gameId)
    {
        var game = await _gameState.LoadAsync(gameId);
        _gameState.EnsureActive(game);

        if (game.CurrentEnemy is null)
            throw new InvalidOperationException("There is no combat to escape from.");

        game.CurrentEnemy = null;
        game.CurrentEnemyId = null;
        game.CurrentEnemyHealth = null;
        await _context.SaveChangesAsync();

        var response = _gameState.CreateResponse(game, "You escape from the fight.");
        return response;
    }
}
