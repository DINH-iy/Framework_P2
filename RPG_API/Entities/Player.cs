using System.ComponentModel.DataAnnotations;

namespace Game.Domain.Entities;

public class Player
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Health { get; set; } = 100;
    public int Attack { get; set; } = 10;
    public int MaxHealth { get; set; } = 100;
}