using Game.Api.Features.Game;
using Game.Api.Models;
using Game.Domain;
using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class MovementService
{
    private readonly GameDbContext _context;
    private readonly GameStateService _gameState;

    public MovementService(GameDbContext context, GameStateService gameState)
    {
        _context = context;
        _gameState = gameState;
    }

    public async Task<GameResponse> MoveAsync(int gameId)
    {
        var game = await _gameState.LoadAsync(gameId);
        _gameState.EnsureActive(game);

        if (game.CurrentEnemy is not null)
            throw new InvalidOperationException("You must finish the current combat first.");

        game.CurrentLevel++;
        game.CurrentLocation = $"Level {game.CurrentLevel}";
        var message = $"You continue to {game.CurrentLocation}.";

        if (game.CurrentLevel == 10)
        {
            game.CurrentRoomType = RoomType.Exit;
            game.IsFinished = true;
            game.IsWon = true;
            message += " You reached the exit and won the game!";
        }
        else if (IsRestLevel(game.CurrentLevel))
        {
            game.CurrentRoomType = RoomType.Rest;
            game.Player.Health = game.Player.MaxHealth;
            message += " You found a rest site and recovered all health.";
        }
        else
        {
            await CreateRandomRoomAsync(game, message);
            message = CreateRoomMessage(game, message);
        }

        await _context.SaveChangesAsync();
        return _gameState.CreateResponse(game, message);
    }

    private async Task CreateRandomRoomAsync(GameEntity game, string message)
    {
        var roomType = ChooseRoomType();
        game.CurrentRoomType = roomType;

        if (roomType == RoomType.Combat)
        {
            var enemies = await _context.Enemies.ToListAsync();
            var enemy = enemies[Random.Shared.Next(enemies.Count)];
            game.CurrentEnemy = enemy;
            game.CurrentEnemyHealth = enemy.MaxHealth;
        }

        if (roomType == RoomType.Treasure)
        {
            var treasureItem = await FindTreasureItemAsync();
            AddItemToPlayer(game, treasureItem);
        }
    }

    private static string CreateRoomMessage(GameEntity game, string message)
    {
        if (game.CurrentRoomType == RoomType.Combat && game.CurrentEnemy is not null)
            return $"{message} A {game.CurrentEnemy.Name} appears!";

        if (game.CurrentRoomType == RoomType.Treasure)
        {
            var newestItem = game.Player.Items.OrderByDescending(item => item.ItemId).First();
            return $"{message} You found a treasure room and received a {newestItem.Item.Name}!";
        }

        return message;
    }

    private async Task<Item> FindTreasureItemAsync()
    {
        var items = await _context.Items.ToListAsync();
        return items[Random.Shared.Next(items.Count)];
    }

    private static void AddItemToPlayer(GameEntity game, Item treasureItem)
    {
        var existingItem = game.Player.Items.FirstOrDefault(item => item.ItemId == treasureItem.Id);

        if (existingItem is not null)
        {
            existingItem.Amount++;
            return;
        }

        game.Player.Items.Add(new PlayerItem
        {
            PlayerId = game.PlayerId,
            ItemId = treasureItem.Id,
            Item = treasureItem,
            Amount = 1
        });
    }

    private static bool IsRestLevel(int level)
    {
        return level == 3 || level == 6 || level == 9;
    }

    private static RoomType ChooseRoomType()
    {
        var randomNumber = Random.Shared.Next(0, 10);

        if (randomNumber < 2)
            return RoomType.Treasure;
        if (randomNumber < 6)
            return RoomType.Combat;

        return RoomType.Normal;
    }
}
