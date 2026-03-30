using System;
using UnityEngine;

[Serializable]
public class EnemyPatrolData
{
    [SerializeField] private float _patrolDistance = 15f;
    [SerializeField] private Vector2Int[] _moveDirections =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    public float PatrolDistance => _patrolDistance;
    public Vector2Int[] MoveDirections => _moveDirections;
}