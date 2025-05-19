using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyComponent : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] protected Enemy enemy;

    protected virtual void Awake()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            enemy = GetComponentInParent<Enemy>();
            
        }
    }
}
