using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicProjectile : Projectile
{

    private bool hasPushed = false;
    public float pushBackAmount = 0.5f;
    private void OnEnable()
    {
        hasPushed = false;
        StartCoroutine(HandleSonicEffect());
    }
    protected override void MoveProjectile() { }

    protected override void RotateToEnemy() { }
    private IEnumerator HandleSonicEffect()
    {
        if (!hasPushed)
        {
            foreach (var enemy in Tower.TargetEnemies)
            {
                if (enemy != null)
                {
                    ApplyDamageAndEffects(enemy);
                    enemy.EnemyEffectController.ApplyPushback(transform.position, this.Tower.CurrentTowerLevel, pushBackAmount);
                }
            }
            hasPushed = true;
        }

        Animator animator = GetComponent<Animator>();
        float animTime = AnimatorUtils.GetCurrentAnimationLength(animator);
        yield return new WaitForSeconds(animTime);


        this.Tower.TowerProjectile.PoolerProjectile.ReturnObjectToPool(this.gameObject);
    }
}
