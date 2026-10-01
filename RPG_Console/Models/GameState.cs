using System.Text.Json.Serialization;

namespace RPG_Console.Models;

public class GameSummary
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = "";
    public string CurrentLocation { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsFinished { get; set; }
    public bool IsWon { get; set; }
}

public class GameState
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string CurrentLocation { get; set; } = "";
    public int? CurrentEnemyId { get; set; }
    public bool IsFinished { get; set; }
    public bool IsWon { get; set; }
    public string Message { get; set; } = "";
    public int DamageDealt { get; set; }
    public int DamageReceived { get; set; }
    public string? PlayerAttack { get; set; }
    public string? EnemyAttack { get; set; }
    public PlayerState? Player { get; set; }
    public EnemyState? Enemy { get; set; }
    public List<ItemState> Inventory { get; set; } = new();
    public MapState Map { get; set; } = new();
}

public class MapState
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int PlayerX { get; set; }
    public int PlayerY { get; set; }
    public List<MapTileState> Tiles { get; set; } = new();
}

public class MapTileState
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsVisited { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsTreasure { get; set; }
    public bool IsExit { get; set; }
}

public class EnemyState
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
}

public class ItemState
{
    public int Slot { get; set; }
    public int Selection { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public int Amount { get; set; }
    public int Value { get; set; }
    public bool IsEquipped { get; set; }
    public bool CanEquip { get; set; }
}
