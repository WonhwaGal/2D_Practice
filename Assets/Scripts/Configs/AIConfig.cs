using System;
using UnityEngine;

namespace PlatformerMVC
{
    [Serializable]
    public struct AIConfig
    {
        public EnemyType enemyType;
        [Range(50,500)]
        public float speed;
        public float minSqrDistanceToTarget;
        public float detectionDistance;
        public Transform[] waypoints;
        public LayerMask targetMask;
        public LayerMask obstructionMask;
    }
    public enum EnemyType
    {
        Patrol = 0,
        Guard = 1
    }
}
