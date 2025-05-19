using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerComponent : MonoBehaviour
{
    [SerializeField] public Tower tower;

    public Tower Tower { get => tower; }

    private void Awake()
    {
        tower = GetComponent<Tower>();
        if (tower == null)
            tower = GetComponentInParent<Tower>();
        if(tower == null)
            tower = FindAnyObjectByType<Tower>();
    }
    public void SetTower(Tower tower)
    {
        this.tower = tower;
    }
}
