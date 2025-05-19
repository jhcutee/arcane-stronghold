using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : TowerComponent
{
    private Enemy _targetEnemy;
    private void FixedUpdate()
    {
        if (_targetEnemy == null || _targetEnemy.EnemyHP.CurrentHp <= 0f || !_targetEnemy.gameObject.activeSelf)
        {
            Tower.TowerProjectile.PoolerProjectile.ReturnObjectToPool(this.gameObject);
            return;
        }

        MoveProjectile();
        RotateToEnemy();
    }
    protected virtual void MoveProjectile()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetEnemy.transform.position,
            Tower.ProjectLineSpeed * Time.deltaTime
        );
    }
    protected virtual void RotateToEnemy()
    {
        Vector3 direction = _targetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetTargetEnemy(Enemy enemy)
    {
        _targetEnemy = enemy;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy hitEnemy))
        {
            if (hitEnemy != _targetEnemy) return;

            Quaternion rot = Quaternion.Euler(0, 0, 180);
            if(Tower.SpawnImpact != null)
            {
                OnHitFx(hitEnemy.transform.position, this.transform.rotation * rot); 
            }

            ApplyDamageAndEffects(hitEnemy);
            this.Tower.TowerProjectile.PoolerProjectile.ReturnObjectToPool(this.gameObject);
        }
    }

    protected virtual void ApplyDamageAndEffects(Enemy enemy)
    {
        if (enemy == null) return;
        if (enemy.HasSpecialEffect(EnemyEffectType.Dodge))
        {
            if (Random.value <= 0.3f)
            {
                return;
            }
        }
        enemy.EnemyHP.TakeHit(this.Tower.CurrentDmg);
    }
    private void OnHitFx(Vector3 pos, Quaternion rot)
    {
        Tower.SpawnImpact.SpawnImpactEffect(pos, rot);
    }
}
