using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Tower", menuName = "ScriptableObject/Tower", order = 2)]
public class TowerSO : ScriptableObject
{
    [Header("Genaral Info")]
    public TowerType towerType;
    public Sprite[] towerIcon;
    public int towerCrytalCost;

    [Header("Attack Setting")]
    public float attackRange;
    public float fireRate;
    public int[] dmgPerLevel;
    public float projectLineSpeed;
    public EffectType specialEffects;

    [Header("Upgrade Setting")]
    public int maxUpgradeLevel;
    public int[] upgradeCrystalCost;
    public int[] upgradeMageBookCost;
}
public enum TowerType
{
    IceTower,
    LightningTower,
    FireTower,
    PosionTower,
    SonicTower,
    LightTower,
}
public enum EffectType
{
    None,
    Slow,     
    Poison,         
    Splash,         
    Knockback,      
    Burn,
}