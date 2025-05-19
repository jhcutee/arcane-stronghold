using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    private Vector3 currentPointPosition;
    private int currentPositionIndex;

    private void Start()
    {
        if (Spawner.Instance != null && Spawner.Instance.WayPoint != null)
        {
            currentPositionIndex = Spawner.Instance.WayPoint.GetNearestPointIndex(this.transform.position);
            currentPointPosition = Spawner.Instance.WayPoint.GetPointPosition(currentPositionIndex);
        }
        else
        {
            Debug.LogError("Spawner or WayPoint is not set up correctly.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, currentPointPosition, Time.deltaTime * 5f);

        if (IsCurrentPointPositionReached())
        {
            UpdatePointPosition();
        }
    }

    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetEnemyState(EnemyState.Stun, enemy);
            }
        }
    }

    private bool IsCurrentPointPositionReached()
    {
        return Vector3.Distance(this.transform.position, currentPointPosition) < 0.1f;
    }

    private void UpdatePointPosition()
    {
        if (currentPositionIndex > 0)
        {
            currentPositionIndex--;
            currentPointPosition = Spawner.Instance.WayPoint.GetPointPosition(currentPositionIndex);
        }
        else
        {
            SelfDestroy();
        }
    }
}
