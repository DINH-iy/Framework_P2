using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities;

public class Player
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Health { get; set; } = 100;
    public int Defense { get; set; } = 0;
    public int Attack { get; set; } = 10;
    public int MaxHealth { get; set; } = 100;
    public int Gold { get; set; }
    public int Experience { get; set; }

    [InverseProperty(nameof(PlayerItem.Player))]
    public virtual ICollection<PlayerItem> Items { get; set; } = new List<PlayerItem>();
}