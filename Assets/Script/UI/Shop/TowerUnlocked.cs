using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TowerUnlocked", menuName = "ScriptableObject/TowerUnlocked", order = 3)]
public class TowerUnlocked : ScriptableObject
{
    public TowerInfoUnLocked[] towersUnlocked;
}
[System.Serializable]
public class TowerInfoUnLocked
{
    public TowerSO towerSO;
    public TowerType towerType => towerSO.towerType;
    public int[] dmgPerLevel => towerSO.dmgPerLevel;
    public Sprite towerSprite => towerSO.towerIcon[0];
    public EffectType specialAbillity => towerSO.specialEffects;
    public int crystalCost => towerSO.towerCrytalCost;
}