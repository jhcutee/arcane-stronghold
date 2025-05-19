using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : Projectile
{
    protected override void ApplyDamageAndEffects(Enemy targetEnemy)
    {
        base.ApplyDamageAndEffects(targetEnemy);

        // Find the nearest enemy from the Tower's TargetEnemies list
        Enemy nearestEnemy = FindNearestEnemy(targetEnemy, Tower.TargetEnemies);

        // Deal 50% damage to the nearest enemy, if found
        if (nearestEnemy != null && nearestEnemy != targetEnemy)
        {
            nearestEnemy.EnemyHP.ApllyDmg((int)(Tower.CurrentDmg * 0.5f));
        }
    }

    private Enemy FindNearestEnemy(Enemy targetEnemy, List<Enemy> targetEnemies)
    {
        Enemy nearestEnemy = null;
        float shortestDistance = float.MaxValue;

        foreach (var enemy in targetEnemies)
        {
            if (enemy == targetEnemy) continue; // Skip the target enemy

            float distance = Vector3.Distance(targetEnemy.transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
