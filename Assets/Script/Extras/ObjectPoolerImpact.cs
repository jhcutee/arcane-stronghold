using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolerImpact : TowerComponent
{
    [Header("Elements")]
    [SerializeField] private GameObject[] impactPrefabs;

    public List<GameObject> impactsPool;
    private GameObject containerPool;
    private int poolSize = 1;
    private int CurrentImpactIndex => Tower.CurrentTowerLevel - 1;

    private void Awake()
    {
        containerPool = new GameObject($"Impact Pool - {gameObject.name}");
        containerPool.transform.parent = transform;
        impactsPool = new List<GameObject>();
    }

    private void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        ClearPool();

        GameObject impactPrefab = impactPrefabs[CurrentImpactIndex];
        for (int i = 0; i < poolSize; i++)
        {
            CreatePrefabs(impactPrefab);
        }
        
    }
    private void ClearPool()
    {
        if (impactsPool == null) return;

        foreach (var impact in impactsPool)
        {
            if (impact != null)
                Destroy(impact);
        }
        impactsPool.Clear();
    }
    public GameObject CreatePrefabs(GameObject impactPrefab)
    {
        GameObject newImpact = Instantiate(impactPrefab);
        newImpact.name = impactPrefab.name;
        newImpact.SetActive(false);
        newImpact.transform.parent = containerPool.transform;
        impactsPool.Add(newImpact);
        return newImpact;
    }
    public GameObject GetInstanceFromPool()
    {
        foreach (var impact in impactsPool)
        {
            if (!impact.activeInHierarchy)
            {
                return impact;
            }
        }
        GameObject impactPrefab = impactPrefabs[CurrentImpactIndex];
        return CreatePrefabs(impactPrefab);
    }

    public void ReturnObjectToPool(GameObject impact)
    {
        Debug.Log($"Returning impact: {impact.name}");
        impact.SetActive(false);
    }
    public void HandleTowerUpgrade()
    {
        CreatePool();   
    }
}
