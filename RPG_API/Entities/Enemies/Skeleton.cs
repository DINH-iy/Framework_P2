namespace Game.Domain.Entities;

public class Skeleton : Enemy
{
    public Skeleton()
    {
        Name = "Skeleton";
        MaxHealth = 40;
        Health = 40;
        Damage = 8;
        Speed = 10;
        RewardGold = 10;
        RewardExperience = 20;
    }
}