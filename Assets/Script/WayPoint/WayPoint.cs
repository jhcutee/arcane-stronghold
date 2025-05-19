using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private Vector3[] points;
    public Vector3[] Points => points;

    private Vector3 _currentPosition;
    public Vector3 CurrentPosition => _currentPosition;
    private bool _isGameStarted = false;
    void Start()
    {
        _isGameStarted = true;
        _currentPosition = this.transform.position;
    }
    public Vector3 GetPointPosition(int index)
    {
        return CurrentPosition + points[index];
    }
    private void OnDrawGizmos()
    {
        if(!_isGameStarted && this.transform.hasChanged)
        {
            _currentPosition = this.transform.position;
        }
        for(int i = 0;i < points.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(points[i] + _currentPosition, 0.5f);
            if(i < points.Length - 1)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(points[i] + _currentPosition, points[i + 1] + _currentPosition);
            }
        }

    }
    public int GetNearestPointIndex(Vector3 position)
    {
        if (points == null || points.Length == 0)
        {
            Debug.LogWarning("WayPoint: No points available.");
            return -1;
        }
        int nearestIndex = -1;
        float nearestDistance = float.MaxValue;
        for (int i = 0; i < points.Length; i++)
        {
            float distance = Vector3.Distance(position, GetPointPosition(i));
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }
        return nearestIndex;
    }
}
