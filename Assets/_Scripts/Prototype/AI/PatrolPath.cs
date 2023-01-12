using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PatrolPath : MonoBehaviour
{
    public List<Transform> patrolPoints = new List<Transform>();

    public int Length { get => patrolPoints.Count; }

    [Header("Gimoz paramaters")]
    public Color pointsColor = Color.blue;
    public float pointSize = 0.3f;
    public Color lineColor = Color.magenta;

    public PathPoint GetClosestPathPoint(Vector2 aiPosition)
    {
        var minDistance = float.MaxValue;
        var index = -1;

        for(int i = 0; i < patrolPoints.Count; i++)
        {
            var tempDistance = Vector2.Distance(aiPosition, patrolPoints[i].position);
            if(tempDistance < minDistance)
            {
                minDistance = tempDistance;
                index = i;
            }
        }
        return new PathPoint(index, patrolPoints[index].position);
    }

    public PathPoint GetNextPathPoint(int index)
    {
        var newIndex = (index + 1) >= patrolPoints.Count ? 0 : index + 1;
        return new PathPoint(newIndex, patrolPoints[newIndex].position);
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints.Count == 0)
            return;
        for(int i = patrolPoints.Count - 1; i > 0; i--)
        {
            if(patrolPoints[i] == null)
            {
                return;
            }

            Gizmos.color = pointsColor;
            Gizmos.DrawSphere(patrolPoints[i].position, pointSize);

            // at least two point to draw
            if (patrolPoints.Count == 1 || i == 0)
                return;

            Gizmos.color = lineColor;
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i - 1].position);

            if (patrolPoints.Count > 2 && i == patrolPoints.Count - 1)
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
        }
    }
}

public struct PathPoint
{
    public int index;
    public Vector2 position;

    public PathPoint(int _index, Vector2 _position)
    {
        index = _index;
        position = _position;
    }
}