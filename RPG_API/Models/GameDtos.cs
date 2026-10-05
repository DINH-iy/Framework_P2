namespace Game.Api.Models;

public class AttackRequest
{
    public AttackType Type { get; set; }
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
    public int CurrentLevel { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsFinished { get; set; }
    public bool IsWon { get; set; }
}

public class GameResponse
{
    public int Id { get; set; }
    public string CurrentLocation { get; set; } = "";
    public int CurrentLevel { get; set; }
    public string CurrentRoomType { get; set; } = "";
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
}

public class PlayerResponse
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Attack { get; set; }
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
    public int AttackBonus { get; set; }
    public int Speed { get; set; }
    public bool IsEquipped { get; set; }
    public bool CanEquip { get; set; }
}
