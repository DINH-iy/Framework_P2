using Game.Api.Models;

namespace Game.Api.Services;

public class CombatService
{
    public AttackResult PerformAttack(int baseDamage, int baseSpeed, AttackType attackType)
    {
        var damage = 0;

        switch (attackType)
        {
            case AttackType.Light:
                damage = Random.Shared.Next(baseDamage - 2, baseDamage + 3);
                break;

            case AttackType.Heavy:
                damage = Random.Shared.Next(baseDamage + 3, baseDamage + 8);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(attackType));
        }

        var speed = baseSpeed;
        if (attackType == AttackType.Light)
            speed += 10;

        return new AttackResult(attackType, Math.Max(1, damage), speed);
    }

    public AttackType ChooseEnemyAttack()
    {
        var randomNumber = Random.Shared.Next(0, 2);
        if (randomNumber == 0)
            return AttackType.Light;

        return AttackType.Heavy;
    }
}

public class AttackResult
{
    public AttackResult(AttackType type, int damage, int speed)
    {
        Type = type;
        Damage = damage;
        Speed = speed;
    }

    public AttackType Type { get; }
    public int Damage { get; }
    public int Speed { get; }
}
