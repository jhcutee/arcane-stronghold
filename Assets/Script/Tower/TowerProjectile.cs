using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjecttile : TowerComponent
{
    [Header("Elements")]
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private ObjectPoolerProjectile poolerProjectile;
    private Coroutine shootCoroutine;

    public ObjectPoolerProjectile PoolerProjectile { get => poolerProjectile;}

    private void Awake()
    {
        poolerProjectile = GetComponent<ObjectPoolerProjectile>();
    }
    public void StartShoot()
    {
        if (shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(ShootRoutine());
        }
    }
    public void StopShootRoutine()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }
    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (Tower.currentTargetEnemy != null && Tower.currentTargetEnemy.EnemyHP.CurrentHp > 0f)
            {
                Tower.SetTowerState(TowerState.Shoot, Tower);
            }

            yield return new WaitForSeconds(Tower.FireRate);
        }
    }
    public void ShootProjectile()
    {
        if (Tower.currentTargetEnemy == null || Tower.currentTargetEnemy.EnemyHP.CurrentHp <= 0f)
            return;

        GameObject newProjectile = poolerProjectile.GetInstanceFromPool();
        newProjectile.transform.position = projectileSpawnPos.position;

        Weapon weapon = Tower.Weapon.GetComponentInChildren<Weapon>(); 
        newProjectile.transform.rotation = weapon.transform.rotation;


        Projectile projectile = newProjectile.GetComponent<Projectile>();
        
        projectile.SetTargetEnemy(Tower.currentTargetEnemy);

        newProjectile.SetActive(true);
    }
}


