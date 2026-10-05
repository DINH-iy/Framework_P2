using Game.Api.Features.Game;
using Game.Api.Models;
using Game.Domain.Entities;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class GameResponseMapper
{
    public GameResponse Create(
        GameEntity game,
        string message = "",
        int damageDealt = 0,
        int damageReceived = 0,
        string? playerAttack = null,
        string? enemyAttack = null)
    {
        var response = new GameResponse
        {
            Id = game.Id,
            CurrentLocation = game.CurrentLocation,
            CurrentLevel = game.CurrentLevel,
            CurrentRoomType = game.CurrentRoomType.ToString(),
            CurrentEnemyId = game.CurrentEnemyId,
            IsFinished = game.IsFinished,
            IsWon = game.IsWon,
            Message = message,
            DamageDealt = damageDealt,
            DamageReceived = damageReceived,
            PlayerAttack = playerAttack,
            EnemyAttack = enemyAttack,
            Player = new PlayerResponse
            {
                Name = game.Player.Name,
                Health = game.Player.Health,
                MaxHealth = game.Player.MaxHealth,
                Attack = game.Player.Attack
            }
        };

        AddEnemyToResponse(game, response);
        return response;
    }

    private static void AddEnemyToResponse(GameEntity game, GameResponse response)
    {
        if (game.CurrentEnemy is null)
            return;

        response.Enemy = new EnemyResponse
        {
            Id = game.CurrentEnemy.Id,
            Name = game.CurrentEnemy.Name,
            Health = game.CurrentEnemyHealth ?? game.CurrentEnemy.MaxHealth,
            MaxHealth = game.CurrentEnemy.MaxHealth,
            Damage = game.CurrentEnemy.Damage,
            Speed = game.CurrentEnemy.Speed
        };
    }

}
