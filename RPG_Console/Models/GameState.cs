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
    public int CurrentLevel { get; set; }
    public string CurrentRoomType { get; set; } = "";
}

public class EnemyState
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
}