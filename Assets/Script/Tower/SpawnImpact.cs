using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnImpact : TowerComponent
{
    public void SpawnImpactEffect(Vector3 pos, Quaternion rot)
    {
        GameObject impact = Tower.ObjectPoolerImpact.GetInstanceFromPool();
        impact.transform.position = pos;
        impact.transform.rotation = rot;
        impact.SetActive(true);
    }
}
