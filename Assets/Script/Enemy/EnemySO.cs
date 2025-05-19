using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Enemy", menuName = "ScriptableObject/Enemies", order = 0)]
public class EnemySO : ScriptableObject
{
    public EnemyType enemyType;
    public Sprite enemyAvatar;
    public int maxHp;
    public float moveSpeed;
    public EnemyEffectType[] specialEffects;
    public int crystalReward;
    public int mageBookReward;
}
public enum EnemyType
{
    Minataur,
    Skeleton,
    Wolf,
    Yokai,
    BossMinataur,
    BossSkeleton,
    BossWolf,
    BossYokai,
}
public enum EnemyEffectType
{
    None,
    ResistFire,
    ImmuneSlow,
    Dodge,
    ResistKnockback,
    DarkFeathers,
}
