using Game.Api.Models;
using Game.Domain.Entities;
using GameEntity = Game.Domain.Entities.Game;

namespace Game.Api.Services;

public class CombatRoundService
{
    private readonly CombatService _combat;

    public CombatRoundService(CombatService combat)
    {
        _combat = combat;
    }

    public CombatRoundResult Resolve(GameEntity game, AttackType playerAttackType)
    {
        var enemy = game.CurrentEnemy!;
        var playerAttack = CreatePlayerAttack(game, playerAttackType);
        var enemyAttack = CreateEnemyAttack(enemy);
        var result = new CombatRoundResult(playerAttack, enemyAttack);

        if (playerAttack.Speed >= enemyAttack.Speed)
        {
            result.Message += ApplyPlayerAttack(game, enemy, playerAttack, out var damageDealt);
            result.DamageDealt = damageDealt;

            if (game.CurrentEnemy is not null && !game.IsFinished)
            {
                result.Message += ApplyEnemyAttack(game, enemy, enemyAttack, out var damageReceived);
                result.DamageReceived = damageReceived;
            }
        }
        else
        {
            result.Message += $"The {enemy.Name} is faster and attacks first. ";
            result.Message += ApplyEnemyAttack(game, enemy, enemyAttack, out var damageReceived);
            result.DamageReceived = damageReceived;

            if (!game.IsFinished)
            {
                result.Message += ApplyPlayerAttack(game, enemy, playerAttack, out var damageDealt);
                result.DamageDealt = damageDealt;
            }
        }

        return result;
    }

    private AttackResult CreatePlayerAttack(GameEntity game, AttackType attackType)
    {
        var equippedItem = game.Player.Items.FirstOrDefault(item => item.IsEquipped);
        var attackPower = game.Player.Attack;
        var attackSpeed = 10;

        if (equippedItem is not null)
        {
            attackPower += equippedItem.Item.AttackBonus;
            attackSpeed += equippedItem.Item.Speed;
        }

        return _combat.PerformAttack(attackPower, attackSpeed, attackType);
    }

    private AttackResult CreateEnemyAttack(Enemy enemy)
    {
        var attackType = _combat.ChooseEnemyAttack();
        return _combat.PerformAttack(enemy.Damage, enemy.Speed, attackType);
    }

    private static string ApplyPlayerAttack(
        GameEntity game,
        Enemy enemy,
        AttackResult attack,
        out int damageDealt)
    {
        damageDealt = attack.Damage;
        game.CurrentEnemyHealth = Math.Max(
            0,
            game.CurrentEnemyHealth.GetValueOrDefault(enemy.MaxHealth) - damageDealt);

        var message = $"You use a {attack.Type} attack and deal {damageDealt} damage to the {enemy.Name}. ";

        if (game.CurrentEnemyHealth <= 0)
        {
            game.CurrentEnemy = null;
            game.CurrentEnemyId = null;
            game.CurrentEnemyHealth = null;
            message += $"The {enemy.Name} is defeated. ";
        }

        return message;
    }

    private static string ApplyEnemyAttack(GameEntity game, Enemy enemy, AttackResult attack, out int damageReceived)
    {
        damageReceived = attack.Damage;
        game.Player.Health = Math.Max(0, game.Player.Health - damageReceived);

        var message = $"The {enemy.Name} uses a {attack.Type} attack and deals {damageReceived} damage. ";

        if (game.Player.Health == 0)
        {
            game.IsFinished = true;
            message += "You have died. ";
        }

        return message;
    }
}

public class CombatRoundResult
{
    public CombatRoundResult(AttackResult playerAttack, AttackResult enemyAttack)
    {
        PlayerAttack = playerAttack;
        EnemyAttack = enemyAttack;
    }

    public AttackResult PlayerAttack { get; }
    public AttackResult EnemyAttack { get; }
    public string Message { get; set; } = "";
    public int DamageDealt { get; set; }
    public int DamageReceived { get; set; }
}
