using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerEnemy : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> dictPoolEnemy = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> dictContainer = new Dictionary<string, GameObject>();

    
    public void InitPool(List<GameObject> enemyPrefabs)
    {
        foreach(GameObject enemyPrefab in enemyPrefabs)
        {
            string key = enemyPrefab.name;
            if (dictPoolEnemy.ContainsKey(key)) continue;

            int poolSize = key.Contains("Boss") ? 5 : 15;

            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject container = new GameObject($"Pool - {key}");
            container.transform.parent = this.transform;
            
            for(int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.name = enemyPrefab.name;
                enemy.transform.SetParent(container.transform);
                enemy.SetActive(false);
                objectPool.Enqueue(enemy);
            }
            
            dictPoolEnemy.Add(key, objectPool);
            dictContainer.Add(key, container);
        }
    }

    public GameObject GetInstanceFromPool(GameObject obj)
    {
        string key = obj.name;
        if(!dictPoolEnemy.ContainsKey(key))
        {
            Debug.Log("Not found Enemy in Pool, Pls Debug");
            return null;
        }
        Queue<GameObject> pool = dictPoolEnemy[key];

        if(pool.Count <= 0)
        {
            GameObject newObj = Instantiate(obj);
            newObj.name = obj.name;
            newObj.transform.parent = dictContainer[key].transform;
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }
        GameObject instance = pool.Dequeue();
        instance.SetActive(true);
        return instance;
    }
    public void ReturnInstanceToPool(GameObject obj)
    {
        obj.SetActive(false);
        string key = obj.name;
        dictPoolEnemy[key].Enqueue(obj);
    }
}