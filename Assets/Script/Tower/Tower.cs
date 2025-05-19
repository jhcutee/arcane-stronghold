using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
public enum TowerState
{
    Idle,
    Shoot,
    UpgradeLv2,
    UpgradeLv3
}

[RequireComponent(typeof(CircleCollider2D))]
public class Tower : MonoBehaviour
{
    [Header("Events")]
    public static Action<TowerState, Tower> onTowerStateChanged;

    [Header("Elements")]
    [SerializeField] private TowerSO towerSo;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Transform weapon;
    [SerializeField] private SpawnWeapon spawnWeapon;
    [SerializeField] private TowerAnimation towerAnimation;
    [SerializeField] private TowerProjecttile towerProjectile;
    [SerializeField] private SpawnImpact spawnImpact;
    [SerializeField] private ObjectPoolerImpact objectPoolerImpact;
    private List<Enemy> targetEnemies;
    public Enemy currentTargetEnemy { get; set; }
    private TowerState towerState;
    public TowerState TowerState => towerState;

    [Header("Settings")]
    [SerializeField] public TowerType towerType;
    [SerializeField] protected int currentLevel;
    [SerializeField] protected float attackRange;
    [SerializeField] protected int currentDmg;
    [SerializeField] protected float fireRate;
    [SerializeField] protected EffectType specialEffects;
    [SerializeField] protected float projectLineSpeed;
    public TowerType TowerType => towerType;
    public int CurrentTowerLevel { get; set; }
    public float AttackRange { get => attackRange; }
    public int CurrentDmg { get => currentDmg; }
    public float FireRate { get => fireRate; }
    public EffectType SpecialEffects { get => specialEffects; }
    public float ProjectLineSpeed { get => projectLineSpeed; }
    public SpawnWeapon SpawnWeapon { get => spawnWeapon;}
    public TowerAnimation TowerAnimation { get => towerAnimation; }
    public TowerProjecttile TowerProjectile { get => towerProjectile;}
    public Transform Weapon { get => weapon; }
    public List<Enemy> TargetEnemies { get => targetEnemies;}
    public SpawnImpact SpawnImpact { get => spawnImpact; }
    public ObjectPoolerImpact ObjectPoolerImpact { get => objectPoolerImpact;}
    public TowerSO TowerSo { get => towerSo;}
    private int maxLevel;
    public int towerValue;
    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        targetEnemies = new List<Enemy>();
        spawnWeapon = GetComponent<SpawnWeapon>();
        CurrentTowerLevel = 1;
        towerAnimation = GetComponent<TowerAnimation>();
        towerProjectile = GetComponent<TowerProjecttile>();
        spawnImpact = GetComponent<SpawnImpact>();
        objectPoolerImpact = GetComponent<ObjectPoolerImpact>();
        GetTowerInfo();
    }
    private void Start()
    {
        circleCollider.radius = AttackRange;
        StartCoroutine(spawnWeapon.HandleSpawnWeapon());
    }
    private void Update()
    {
        GetCurrentTargetEnemy();
    }
    public void UpgradeTower(Tower tower)
    {
        if (tower.CurrentTowerLevel == 1)
        {
            tower.SetTowerState(TowerState.UpgradeLv2, tower);
            tower.towerValue += tower.TowerSo.upgradeCrystalCost[0];
            CurrencyManager.Instance.RemoveCrystal(tower.TowerSo.upgradeCrystalCost[0]);
            CurrencyManager.Instance.RemoveMagebook(tower.TowerSo.upgradeMageBookCost[0]);
        }
        else if (tower.CurrentTowerLevel == 2)
        {
            tower.SetTowerState(TowerState.UpgradeLv3, tower);
            tower.towerValue += tower.TowerSo.upgradeCrystalCost[1];
            CurrencyManager.Instance.RemoveCrystal(tower.TowerSo.upgradeCrystalCost[1]);
            CurrencyManager.Instance.RemoveMagebook(tower.TowerSo.upgradeMageBookCost[1]);
        }
        UpgradeManager.instance.HideUpgradeCanvas();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemyTarget = collision.GetComponent<Enemy>();
            TargetEnemies.Add(enemyTarget);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemyTarget = collision.GetComponent<Enemy>();
            if (TargetEnemies.Contains(enemyTarget))
            {
                TargetEnemies.Remove(enemyTarget);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.DrawWireSphere(this.transform.position, AttackRange);
        }
    }
    public void SetTowerState(TowerState state, Tower tower)
    {
        this.towerState = state;
        onTowerStateChanged?.Invoke(state, this);
    }
    public void GetTowerInfo()
    {
        towerType = TowerSo.towerType;
        attackRange = TowerSo.attackRange;
        GetTowerDmg();
        fireRate = TowerSo.fireRate;
        specialEffects = TowerSo.specialEffects;
        projectLineSpeed = TowerSo.projectLineSpeed;
    }
    public void GetTowerDmg()
    {
        int index = CurrentTowerLevel - 1;
        currentDmg = TowerSo.dmgPerLevel[index];
    }
    private void GetCurrentTargetEnemy()
    {
        if(TargetEnemies.Count <= 0)
        {
            currentTargetEnemy = null;
            return;
        }
        currentTargetEnemy = TargetEnemies[0];
        if(currentTargetEnemy.EnemyHP.CurrentHp <= 0f)
        {
            TargetEnemies.Remove(currentTargetEnemy);
        }
    }
}
