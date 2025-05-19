using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SpawnModes
{
    Fixed = 1,
    Random = 2,
}

public class Spawner : MonoBehaviour
{
    private static Spawner instance;
    public static Spawner Instance { get => instance; }
    public ObjectPoolerEnemy ObjectPooler { get => objectPooler; }
    public WayPoint WayPoint { get => wayPoint; }
    public int CurrentWaveIndex { get => currentWaveIndex; }

    [Header("Elements")]
    [SerializeField] private ObjectPoolerEnemy objectPooler;
    [SerializeField] private WayPoint wayPoint;
    [SerializeField] public EnemyWaveSO enemyWavesInCurrentLevel;
    [SerializeField] private GameObject spawnEffect;

    [Header("Settings")]
    [SerializeField] private SpawnModes spawnModes = SpawnModes.Fixed;
    [SerializeField] private int maxSpawn = 10;

    [Header("Fixed Spawn Settings")]
    [SerializeField] private float delayBetweenSpawns;

    [Header("Random Spawn Settings")]
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;

    [Header("Wave Settings")]
    [SerializeField] private float delayBetweenWaves = 5f;

    private float spawnTimer;
    private int enemiesSpawned;
    private int enemiesRemaining;
    private int currentWaveIndex = 0;
    private List<GameObject> currentWaveEnemies;
    private bool isSpawningWave = false;
    private bool canRewardCrystal = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        objectPooler = GetComponentInChildren<ObjectPoolerEnemy>();
        wayPoint = GetComponentInChildren<WayPoint>();
    }
    private void OnEnable()
    {
        Enemy.onEndPointPositionReached += OnEnemyReachedEndpoint;
        Enemy.onEnemyStateChanged += OnEnemyStateChanged;
    }

    private void OnDisable()
    {
        Enemy.onEndPointPositionReached -= OnEnemyReachedEndpoint;
        Enemy.onEnemyStateChanged -= OnEnemyStateChanged;
    }

    private void Start()
    {
        StartCoroutine(DelayStartFirstWave());
    }

    private IEnumerator DelayStartFirstWave()
    {
        yield return new WaitForSeconds(10f);
        LoadWave(CurrentWaveIndex);
        ObjectPooler.InitPool(currentWaveEnemies);
        UIManager.instance.ShowWaveCounterLabel(currentWaveIndex);
    }

    private void Update()
    {
        HandleSpawning();
    }

    private void HandleSpawning()
    {
        if (!isSpawningWave) return;

        if (enemiesSpawned < maxSpawn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnTimer = GetSpawnTimer();
                StartCoroutine(HandleSpawnEnemy());
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemiesSpawned >= currentWaveEnemies.Count) return;
        GameObject enemyToSpawn = currentWaveEnemies[enemiesSpawned];
        GameObject enemy = ObjectPooler.GetInstanceFromPool(enemyToSpawn);

        if (enemy != null)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.WayPoint = WayPoint;
            enemyComponent.SetEnemyState(EnemyState.Walk, enemyComponent);
            enemyComponent.SetMoveSpeed(1f);
            enemyComponent.ResetWayPoint();
            enemy.transform.position = WayPoint.GetPointPosition(0);
        }

        enemiesSpawned++;
    }
    private IEnumerator HandleSpawnEnemy()
    {
        spawnEffect.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        SpawnEnemy();
        yield return new WaitForSeconds(0.25f);
        spawnEffect.SetActive(false);
    }
    private IEnumerator DelayBetweenWaves()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        

        if (currentWaveIndex >= enemyWavesInCurrentLevel.enemyWaves.Count)
        {
            HandleAllWavesCompleted();
            yield break;
        }

        LoadWave(currentWaveIndex);
        ObjectPooler.InitPool(currentWaveEnemies);
    }

    private void RewardPlayerForWave()
    {
        if (canRewardCrystal && CurrentWaveIndex < enemyWavesInCurrentLevel.enemyWaves.Count)
        {
            UIManager.instance.ShowWaveFinishedLabel();
            int waveReward = enemyWavesInCurrentLevel.enemyWaves[CurrentWaveIndex].crystalReward;
            CurrencyManager.Instance.AddCrystal(waveReward);
        }
        if(currentWaveIndex == enemyWavesInCurrentLevel.enemyWaves.Count - 1)
        {
            UIManager.instance.ShowTheLastWaveLabel();
        }
        else
        {
            UIManager.instance.ShowWaveCounterLabel(currentWaveIndex);
        }
    }

    private void OnEnemyReachedEndpoint()
    {
        enemiesRemaining--;
        canRewardCrystal = false;
        CheckWaveCompletion();
    }

    private void OnEnemyStateChanged(EnemyState enemyState, Enemy enemy)
    {
        if (enemyState == EnemyState.Die)
        {
            enemiesRemaining--;
            CheckWaveCompletion();
        }
    }

    private void CheckWaveCompletion()
    {
        if (enemiesRemaining <= 0)
        {
            currentWaveIndex++;
            RewardPlayerForWave();
            isSpawningWave = false;
            StartCoroutine(DelayBetweenWaves());
        }
    }

    private float GetSpawnTimer()
    {
        return spawnModes == SpawnModes.Fixed ? delayBetweenSpawns : Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void LoadWave(int index)
    {
        if (index >= enemyWavesInCurrentLevel.enemyWaves.Count)
        {
            return;
        }

        currentWaveEnemies = enemyWavesInCurrentLevel.enemyWaves[index].enemyPrefabs;
        maxSpawn = currentWaveEnemies.Count;
        enemiesRemaining = currentWaveEnemies.Count;
        enemiesSpawned = 0;
        spawnTimer = 0;
        isSpawningWave = true;
        canRewardCrystal = true;
    }

    private void HandleAllWavesCompleted()
    {
        isSpawningWave = false;
        if (LevelManager.instance.CurrentLevel < 3) 
        {
            GameManager.instance.SetGameState(GameState.LevelCompleted);
        }
        else
        {
            GameManager.instance.SetGameState(GameState.GameWin);
        }
    }
}
