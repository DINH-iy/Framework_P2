using System.Text.Json.Serialization;

namespace RPG_Console.Models;

public class PlayerState
{
    public string Name { get; set; } = "";
    public int Health { get; set; }
    public int MaxHealth { get; set; }
}
