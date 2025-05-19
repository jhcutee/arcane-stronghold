using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeapon : TowerComponent
{
    [Header("Elements")]
    [SerializeField] private Transform spawnWeaponPos;
    [SerializeField] private List<GameObject> weaponPrefabs; 
    public Weapon SpawnedWeapon { get; private set; }
    private float timeSpawn = 0.5f;
    public IEnumerator HandleSpawnWeapon()
    {
        if (spawnWeaponPos.childCount > 0)
        {
            for (int i = 0; i < spawnWeaponPos.childCount; i++)
            {
                Destroy(spawnWeaponPos.GetChild(i).gameObject);
            }
        }
        int index = Tower.CurrentTowerLevel - 1;

        Vector3 spawnPos = Vector3.zero;

        if (index == 1)
        {
            timeSpawn = 0.6f;
            spawnPos.y += 0.15f;
        }
        else if (index == 2)
        {
            timeSpawn = 0.6f;
            spawnPos.y += 0.3f;
        }

        yield return new WaitForSeconds(timeSpawn);

        GameObject newWeapon = Instantiate(weaponPrefabs[index]);
        newWeapon.transform.SetParent(spawnWeaponPos.transform);
        newWeapon.transform.localPosition = spawnPos;
        Weapon SpawnedWeapon = newWeapon.GetComponent<Weapon>();
        if (SpawnedWeapon != null)
        {
            SpawnedWeapon.SetTower(Tower);
        }
        Tower.TowerProjectile.StartShoot();
    }
}
