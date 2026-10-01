using Game.Api.Models;
using Game.Api.Features.Game;
using Game.Domain;
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

    public async Task<GameResponse> MoveAsync(int gameId, MoveDirection direction)
    {
        var game = await _gameState.LoadAsync(gameId);
        _gameState.EnsureActive(game);

        if (game.CurrentEnemy is not null)
            throw new InvalidOperationException("You must finish the current combat first.");

        var nextPosition = GetNextPosition(game.PositionX, game.PositionY, direction);
        game.PositionX = nextPosition.X;
        game.PositionY = nextPosition.Y;
        game.CurrentLocation = $"Room ({game.PositionX}, {game.PositionY})";
        AddVisitedRoom(game);

        var message = $"You move to {game.CurrentLocation}.";

        if (game.PositionX == 4 && game.PositionY == 4)
        {
            game.CurrentRoomType = RoomType.Exit;
            game.IsFinished = true;
            game.IsWon = true;
            message += " You found the exit and won the game!";

            await _context.SaveChangesAsync();
            return _gameState.CreateResponse(game, message);
        }

        var roomType = ChooseRoomType();
        game.CurrentRoomType = roomType;

        if (roomType == RoomType.Combat)
        {
            var enemies = await _context.Enemies.ToListAsync();
            var enemy = enemies[Random.Shared.Next(enemies.Count)];
            game.CurrentEnemy = enemy;
            game.CurrentEnemyHealth = enemy.MaxHealth;
            message += $" A {enemy.Name} appears!";
        }

        if (roomType == RoomType.Treasure)
        {
            game.Player.Gold += 10;
            AddTreasureRoom(game);
            message += " You found a treasure room and gained 10 gold!";
        }

        await _context.SaveChangesAsync();
        return _gameState.CreateResponse(game, message);
    }

    private static (int X, int Y) GetNextPosition(int currentX, int currentY, MoveDirection direction)
    {
        var nextPosition = (X: currentX, Y: currentY);

        switch (direction)
        {
            case MoveDirection.Up:
                nextPosition.Y--;
                break;
            case MoveDirection.Down:
                nextPosition.Y++;
                break;
            case MoveDirection.Left:
                nextPosition.X--;
                break;
            case MoveDirection.Right:
                nextPosition.X++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction));
        }

        if (nextPosition.X < 0 || nextPosition.X > 4
            || nextPosition.Y < 0 || nextPosition.Y > 4)
            throw new InvalidOperationException("You cannot move outside the map.");

        return nextPosition;
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

    private static void AddVisitedRoom(GameEntity game)
    {
        var coordinate = $"{game.PositionX},{game.PositionY}";
        var visitedRooms = game.VisitedRooms.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (!visitedRooms.Contains(coordinate))
            visitedRooms.Add(coordinate);

        game.VisitedRooms = string.Join(';', visitedRooms);
    }

    private static void AddTreasureRoom(GameEntity game)
    {
        var coordinate = $"{game.PositionX},{game.PositionY}";
        var treasureRooms = game.TreasureRooms.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (!treasureRooms.Contains(coordinate))
            treasureRooms.Add(coordinate);

        game.TreasureRooms = string.Join(';', treasureRooms);
    }
}
