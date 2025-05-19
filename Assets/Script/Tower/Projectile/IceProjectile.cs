using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    [SerializeField] private float slowValue  = 0.3f; 
    protected override void ApplyDamageAndEffects(Enemy enemy)
    {
        base.ApplyDamageAndEffects(enemy);

        if (enemy.HasSpecialEffect(EnemyEffectType.ImmuneSlow)) return;
        if (!enemy.EnemyEffectController.HasEffect(EffectType.Slow))
        {
            enemy.EnemyEffectController.AddEffect(new EnemyEffectData(EffectType.Slow, slowValue, 5f));
        }
    }
}
