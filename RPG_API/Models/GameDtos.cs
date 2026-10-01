namespace Game.Api.Models;

public class AttackRequest
{
    public AttackType Type { get; set; }
}

public class MoveRequest
{
    public MoveDirection Direction { get; set; }
}

public class EquipItemRequest
{
    public int Selection { get; set; }
}

public class StartGameRequest
{
    public required string PlayerName { get; set; }
}

public class GameSummaryResponse
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = "";
    public string CurrentLocation { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsFinished { get; set; }
    public bool IsWon { get; set; }
}

public class GameResponse
{
    public int Id { get; set; }
    public string CurrentLocation { get; set; } = "";
    public int? CurrentEnemyId { get; set; }
    public bool IsFinished { get; set; }
    public bool IsWon { get; set; }
    public string Message { get; set; } = "";
    public int DamageDealt { get; set; }
    public int DamageReceived { get; set; }
    public string? PlayerAttack { get; set; }
    public string? EnemyAttack { get; set; }
    public PlayerResponse Player { get; set; } = new();
    public EnemyResponse? Enemy { get; set; }
    public List<ItemResponse> Inventory { get; set; } = new();
    public MapResponse Map { get; set; } = new();
}

public class MapResponse
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int PlayerX { get; set; }
    public int PlayerY { get; set; }
    public List<MapTileResponse> Tiles { get; set; } = new();
}

public class MapTileResponse
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsVisited { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsTreasure { get; set; }
    public bool IsExit { get; set; }
}

public class PlayerResponse
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Gold { get; set; }
    public int Experience { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
}

public class EnemyResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
    public int Speed { get; set; }
}

public class ItemResponse
{
    public int Slot { get; set; }
    public int Selection { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public int Amount { get; set; }
    public int Value { get; set; }
    public int Speed { get; set; }
    public bool IsEquipped { get; set; }
    public bool CanEquip { get; set; }
}
