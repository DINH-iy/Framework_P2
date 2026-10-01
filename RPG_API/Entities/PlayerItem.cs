using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Game.Domain.Entities;

[PrimaryKey(nameof(PlayerId), nameof(ItemId))]
public class PlayerItem
{
    [ForeignKey(nameof(Player))]
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;

    [ForeignKey(nameof(Item))]
    public int ItemId { get; set; }
    public virtual Item Item { get; set; } = null!;
    public int Amount { get; set; }
    public bool IsEquipped { get; set; }
}