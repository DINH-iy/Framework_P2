using Game.Api.Models;
using Game.Api.Features.Game;
using Game.Domain;
using Game.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class InventoryService
{
    private readonly GameDbContext _context;
    private readonly GameStateService _gameState;

    public InventoryService(GameDbContext context, GameStateService gameState)
    {
        _context = context;
        _gameState = gameState;
    }

    public async Task<GameResponse> UseItemAsync(int gameId, int slot)
    {
        var game = await _gameState.LoadAsync(gameId);
        _gameState.EnsureActive(game);

        var playerItem = await GetInventorySlotAsync(game, slot);
        if (playerItem is null || playerItem.Amount <= 0)
            throw new InvalidOperationException("There is no item in that inventory slot.");

        if (playerItem.Item.Type != ItemType.Healing)
            throw new InvalidOperationException("That item cannot be used yet.");

        game.Player.Health = Math.Min(game.Player.MaxHealth, game.Player.Health + playerItem.Item.Value);
        playerItem.Amount--;
        await _context.SaveChangesAsync();

        return _gameState.CreateResponse(game, $"You use a {playerItem.Item.Name}.");
    }

    public async Task<GameResponse> EquipItemAsync(int gameId, int selection)
    {
        var game = await _gameState.LoadAsync(gameId);
        _gameState.EnsureActive(game);

        if (game.CurrentEnemy is not null)
            throw new InvalidOperationException("You cannot equip items during combat.");

        var equipableItems = await GetEquipableItemsAsync(game);
        if (selection < 1 || selection > equipableItems.Count)
            throw new InvalidOperationException("That equipable item selection is not valid.");

        var selectedItem = equipableItems[selection - 1];
        foreach (var item in game.Player.Items)
            item.IsEquipped = false;

        selectedItem.IsEquipped = true;
        await _context.SaveChangesAsync();

        return _gameState.CreateResponse(game, $"You equip the {selectedItem.Item.Name}.");
    }

    public async Task<List<ItemResponse>> GetEquipableItemsAsync(int gameId)
    {
        var game = await _gameState.LoadAsync(gameId);
        var equipableItems = await GetEquipableItemsAsync(game);
        var response = new List<ItemResponse>();

        for (var index = 0; index < equipableItems.Count; index++)
        {
            var item = equipableItems[index];
            response.Add(new ItemResponse
            {
                Selection = index + 1,
                Name = item.Item.Name,
                Type = item.Item.Type.ToString(),
                Amount = item.Amount,
                Value = item.Item.Value,
                Speed = item.Item.Speed,
                IsEquipped = item.IsEquipped,
                CanEquip = true
            });
        }

        return response;
    }

    private async Task<List<PlayerItem>> GetEquipableItemsAsync(GameEntity game)
    {
        return await _context.PlayerItems
            .Include(item => item.Item)
            .Where(item => item.PlayerId == game.PlayerId
                && item.Amount > 0
                && item.Item.Type == ItemType.Equipment)
            .OrderBy(item => item.ItemId)
            .Take(4)
            .ToListAsync();
    }

    private async Task<PlayerItem?> GetInventorySlotAsync(GameEntity game, int slot)
    {
        if (slot < 1 || slot > 4)
            throw new ArgumentOutOfRangeException(nameof(slot), "Inventory slots are numbered from 1 to 4.");

        var items = await _context.PlayerItems
            .Include(item => item.Item)
            .Where(item => item.PlayerId == game.PlayerId)
            .OrderBy(item => item.ItemId)
            .Take(4)
            .ToListAsync();

        if (slot > items.Count)
            return null;

        return items[slot - 1];
    }
}
