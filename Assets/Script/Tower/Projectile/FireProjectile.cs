using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile
{
    [SerializeField] private int burnDamage = 5;
    private float burnDuration = 3f;
    protected override void ApplyDamageAndEffects(Enemy enemy)
    {
        base.ApplyDamageAndEffects(enemy);
        if (enemy.HasSpecialEffect(EnemyEffectType.ResistFire)) return;
        if (!enemy.EnemyEffectController.HasEffect(EffectType.Burn))
        {
            enemy.EnemyEffectController.AddEffect(new EnemyEffectData(EffectType.Burn, burnDamage, burnDuration));
        }

    }
}
