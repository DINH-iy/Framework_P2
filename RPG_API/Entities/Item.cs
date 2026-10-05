using Game.Api.Features.Game;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities;

public class Item
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required ItemType Type { get; set; }
    public int AttackBonus { get; set; }
    public int Speed { get; set; }

    [InverseProperty(nameof(PlayerItem.Item))]
    public virtual ICollection<PlayerItem> Players { get; set; } = new List<PlayerItem>();
}