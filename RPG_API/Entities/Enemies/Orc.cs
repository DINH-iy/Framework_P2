namespace Game.Domain.Entities;

public class Orc : Enemy
{
    public Orc()
    {
        Name = "Orc";
        MaxHealth = 70;
        Health = 70;
        Damage = 14;
        Speed = 6;
    }
}