namespace Game.Domain.Entities;

public class Goblin : Enemy
{
    public Goblin()
    {
        Name = "Goblin";
        MaxHealth = 30;
        Health = 30;
        Damage = 6;
        Speed = 14;
        RewardGold = 15;
        RewardExperience = 15;
    }
}