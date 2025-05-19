using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProjectile : Projectile
{
    protected override void ApplyDamageAndEffects(Enemy enemy)
    {
        if (enemy == null) return;
        if (enemy.HasSpecialEffect(EnemyEffectType.Dodge))
        {
            if (Random.value <= 0.3f)
            {
                return;
            }
        }
        if (enemy.HasSpecialEffect(EnemyEffectType.DarkFeathers))
        {
            enemy.EnemyHP.TakeHit(this.Tower.CurrentDmg/2);
            
            return;
        }
        enemy.EnemyHP.TakeHit(this.Tower.CurrentDmg);
    }
}
