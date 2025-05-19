using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSpear : MonoBehaviour
{
    public List<Enemy> enemiesInRange;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemyInRange = collision.GetComponent<Enemy>();
            if (enemyInRange != null && !enemiesInRange.Contains(enemyInRange))
            {
                enemiesInRange.Add(enemyInRange);
            }
        }
    }
    public void KillEnemyInRange()
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                enemy.EnemyHP.TakeHit(enemy.EnemyHP.CurrentHp);
            }
        }
        enemiesInRange.Clear();
    }
    public void SeflDestroy()
    {
        Destroy(this.gameObject);
    }
}
