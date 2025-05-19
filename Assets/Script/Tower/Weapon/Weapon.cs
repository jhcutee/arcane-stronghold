using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : TowerComponent
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Tower.onTowerStateChanged += HandleWeaponAnimation;
    }
    private void OnDisable()
    {
        Tower.onTowerStateChanged -= HandleWeaponAnimation;
    }
    private void Update()
    {
        RotateWeaponToTargetEnemy();
    }
    private void HandleWeaponAnimation(TowerState state, Tower tower)
    {
        if(state == TowerState.Shoot && this.Tower == tower)
        {
            StartCoroutine(HandleShootAnim());
        }
    }
    private void PlayShoot()
    {
        animator.SetTrigger("Shoot");
    }
    public IEnumerator HandleShootAnim()
    {
        PlayShoot();
        yield return new WaitForSeconds(0.2f);
        Tower.TowerProjectile.ShootProjectile();
        yield return new WaitForSeconds(AnimatorUtils.GetCurrentAnimationLength(this.animator) - 0.2f);
        Tower.SetTowerState(TowerState.Idle, this.Tower);
    }
    private void RotateWeaponToTargetEnemy()    
    {
        if (Tower == null ) return;
        if (Tower.TowerType == TowerType.LightTower || Tower.TowerType == TowerType.SonicTower || Tower.TowerType == TowerType.LightningTower)
        {
            return;
        }
        if (Tower.currentTargetEnemy == null) return;
        Vector3 direction = Tower.currentTargetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
