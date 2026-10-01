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
                Gold = game.Player.Gold,
                Experience = game.Player.Experience,
                Attack = game.Player.Attack,
                Defense = game.Player.Defense
            }
        };

        AddEnemyToResponse(game, response);
        AddInventoryToResponse(game, response);
        response.Map = CreateMap(game);

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

    private static void AddInventoryToResponse(GameEntity game, GameResponse response)
    {
        var inventoryItems = game.Player.Items
            .OrderBy(item => item.ItemId)
            .Take(4)
            .ToList();

        for (var index = 0; index < inventoryItems.Count; index++)
        {
            var item = inventoryItems[index];
            response.Inventory.Add(new ItemResponse
            {
                Slot = index + 1,
                Selection = index + 1,
                Name = item.Item.Name,
                Type = item.Item.Type.ToString(),
                Amount = item.Amount,
                Value = item.Item.Value,
                Speed = item.Item.Speed,
                IsEquipped = item.IsEquipped,
                CanEquip = item.Item.Type == ItemType.Equipment
            });
        }
    }

    private static MapResponse CreateMap(GameEntity game)
    {
        var visitedRooms = ReadCoordinates(game.VisitedRooms);
        var treasureRooms = ReadCoordinates(game.TreasureRooms);
        var map = new MapResponse
        {
            Width = 5,
            Height = 5,
            PlayerX = game.PositionX,
            PlayerY = game.PositionY
        };

        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                var coordinate = $"{x},{y}";
                map.Tiles.Add(new MapTileResponse
                {
                    X = x,
                    Y = y,
                    IsVisited = visitedRooms.Contains(coordinate),
                    IsCurrent = x == game.PositionX && y == game.PositionY,
                    IsTreasure = treasureRooms.Contains(coordinate),
                    IsExit = x == 4 && y == 4
                });
            }
        }

        return map;
    }

    private static HashSet<string> ReadCoordinates(string coordinates)
    {
        return coordinates
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet();
    }
}
