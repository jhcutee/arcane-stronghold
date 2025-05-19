using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : Projectile
{
    private float poisonDuration = 5f;
    protected override void ApplyDamageAndEffects(Enemy enemy)
    {
        base.ApplyDamageAndEffects(enemy);
        if (!enemy.EnemyEffectController.HasEffect(EffectType.Poison))
        {
            enemy.EnemyEffectController.AddEffect(new EnemyEffectData(EffectType.Poison, this.Tower.CurrentDmg, poisonDuration));
        }
    }
}
