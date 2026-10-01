using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game.Domain.Entities;

public abstract class Enemy
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Speed { get; set; }
    public int RewardGold { get; set; }
    public int RewardExperience { get; set; }

    [InverseProperty(nameof(Game.CurrentEnemy))]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}