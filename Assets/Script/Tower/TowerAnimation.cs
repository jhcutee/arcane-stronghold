using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimation : TowerComponent
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        Tower.onTowerStateChanged += AnimationTowerChanged;
    }
    private void OnDisable()
    {
        Tower.onTowerStateChanged -= AnimationTowerChanged;
    }
    private void AnimationTowerChanged(TowerState state, Tower tower)
    {
        if(state == TowerState.UpgradeLv2 && this.Tower == tower)
        {
            StartCoroutine(HandlePlayUpgradeLevel2());
        }
        if (state == TowerState.UpgradeLv3 && this.Tower == tower)
        {
            StartCoroutine(HandlePlayUpgradeLevel3());
        }
    }
    private void PlayUpgradeLevel2()
    {
        animator.SetTrigger("UpgradeLevel2");
    }
    private void PlayUpgradeLevel3()
    {
        animator.SetTrigger("UpgradeLevel3");
    }
    private IEnumerator HandlePlayUpgradeLevel2()
    {
        PlayUpgradeLevel2();
        Tower.CurrentTowerLevel = 2;
        Tower.GetTowerDmg();
        Tower.TowerProjectile.PoolerProjectile.HandleTowerUpgrade();
        if (Tower.ObjectPoolerImpact != null)
        {
            Tower.ObjectPoolerImpact.HandleTowerUpgrade();
        }
        Tower.TowerProjectile.StopShootRoutine();
        yield return Tower.SpawnWeapon.StartCoroutine(Tower.SpawnWeapon.HandleSpawnWeapon());
        Tower.TowerProjectile.StartShoot();

        yield return new WaitForSeconds(AnimatorUtils.GetCurrentAnimationLength(this.animator));
        Tower.SetTowerState(TowerState.Idle,this.Tower);
    }
    private IEnumerator HandlePlayUpgradeLevel3()
    {
        PlayUpgradeLevel3(); 
        Tower.CurrentTowerLevel = 3;
        Tower.GetTowerDmg();
        Tower.TowerProjectile.PoolerProjectile.HandleTowerUpgrade();
        if(Tower.ObjectPoolerImpact != null)
        {
            Tower.ObjectPoolerImpact.HandleTowerUpgrade();
        }
        Tower.TowerProjectile.StopShootRoutine();
        yield return Tower.SpawnWeapon.StartCoroutine(Tower.SpawnWeapon.HandleSpawnWeapon());
        Tower.TowerProjectile.StartShoot();

        yield return new WaitForSeconds(AnimatorUtils.GetCurrentAnimationLength(this.animator));
        Tower.SetTowerState(TowerState.Idle,this.Tower);
    }
}
