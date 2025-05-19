using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolerProjectile : TowerComponent
{
    [Header("Elements")]
    [SerializeField] private GameObject[] projectilePrefabs; 

    public List<GameObject> projectilesPool;
    private GameObject containerPool;
    private int poolSize = 1;

    private int CurrentProjectileIndex => Tower.CurrentTowerLevel - 1; 

    private void Awake()
    {
        containerPool = new GameObject($"Pool - {gameObject.name}");
        containerPool.transform.parent = transform;
        projectilesPool = new List<GameObject>();
    }

    private void Start()
    {
        CreatePool();
    }
    private void CreatePool()
    {
        ClearPool();

        GameObject prefabToSpawn = projectilePrefabs[CurrentProjectileIndex];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newProjectile = Instantiate(prefabToSpawn);
            newProjectile.SetActive(false);
            newProjectile.transform.parent = containerPool.transform;

            Projectile projectile = newProjectile.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetTower(Tower);
            }
            projectilesPool.Add(newProjectile);
        }
    }

    private void ClearPool()
    {
        if (projectilesPool == null) return;

        foreach (var projectile in projectilesPool)
        {
            if (projectile != null)
                Destroy(projectile);
        }
        projectilesPool.Clear();
    }

    public GameObject GetInstanceFromPool()
    {
        foreach (var proj in projectilesPool)
        {
            if (!proj.activeInHierarchy)
            {
                return proj;
            }
        }

        GameObject prefabToSpawn = projectilePrefabs[CurrentProjectileIndex];
        GameObject newProjectile = Instantiate(prefabToSpawn);
        newProjectile.SetActive(false);
        newProjectile.transform.parent = containerPool.transform;
        projectilesPool.Add(newProjectile);
        return newProjectile;
    }

    public void ReturnObjectToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }

    public void HandleTowerUpgrade()
    {
        CreatePool();
    }
}