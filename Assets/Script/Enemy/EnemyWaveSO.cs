using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Enemy Waves", menuName = "ScriptableObject/EnemyWaves", order = 1)]
public class EnemyWaveSO : ScriptableObject
{
    public List<EnemyWave> enemyWaves;
}
[System.Serializable]
public class EnemyWave
{
    public List<GameObject> enemyPrefabs;
    public int crystalReward;
}
