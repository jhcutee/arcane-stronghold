using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            Spawner.Instance.ObjectPooler.ReturnInstanceToPool(enemy.gameObject);
            enemy.EnemyHP.ApllyDmg(1000);
        }
    }
    public void SeftDestroy()
    {
        Destroy(this.gameObject);
    }
}
