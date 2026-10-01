using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Game.Api.Features.Game;

namespace Game.Domain.Entities;

public class Game
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Player))]
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;
    public string CurrentLocation { get; set; } = "";
    public int PositionX { get; set; } = 2;
    public int PositionY { get; set; } = 2;
    public string VisitedRooms { get; set; } = "2,2";
    public string TreasureRooms { get; set; } = "";
    public RoomType CurrentRoomType { get; set; } = RoomType.Normal;

    [ForeignKey(nameof(CurrentEnemy))]
    public int? CurrentEnemyId { get; set; }
    public virtual Enemy? CurrentEnemy { get; set; }
    public int? CurrentEnemyHealth { get; set; }
    public bool IsFinished { get; set; }
    public bool IsWon { get; set; }
}
