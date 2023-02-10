using System;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class EnemyModel
    {
        public EnemyType _type;
        private Transform _fromPos;
        private Transform _target;
        private Transform _pointTarget;
        private readonly AIConfig _config;
        private int _xDir = 1;
        private int _currentPointIndex;
        private bool playerCloseToAttack;
        private const float _Ythreshold = 4.5f;

        public int XDir { get => _xDir; set => _xDir = value; }
        public Transform Target { get => _target; set => _target = value; }

        public EnemyModel(Transform fromPos, Transform target, AIConfig config)
        {
            _fromPos = fromPos;
            Target = target;
            _config = config;
            _type = _config.enemyType;
            if (_config.enemyType == EnemyType.Patrol)
            {
                _pointTarget = GetNextWaypoint();
            }
        }
        private bool PlayerDetected()
        {
            if (Vector2.SqrMagnitude(Target.position - _fromPos.position) > _config.detectionDistance) return false;
            else return true;
        }
        public bool PlayerFound()
        {
            if (PlayerDetected())
            {
                Vector2 direction = Target.position - _fromPos.position;
                float rayLength = direction.magnitude;
                bool haveObstructions = Physics2D.Raycast(_fromPos.position, direction, rayLength, _config.obstructionMask);
                if (!haveObstructions)
                {
                    playerCloseToAttack = Physics2D.Raycast(_fromPos.position, direction, rayLength, _config.targetMask);
                    if (playerCloseToAttack && direction.y < _Ythreshold && Mathf.Abs(direction.x) > 2.0f)
                    {
                        if (direction.x > 0)
                        {
                            XDir = 1;
                        }
                        else if (direction.x < 0)
                        {
                            XDir = -1;
                        }
                    }
                    else
                    {
                        playerCloseToAttack = false;
                        if (_fromPos.gameObject.GetComponent<EnemyView>()._lastScale.x > 0) XDir = 1;  //
                        else XDir = -1;
                    }
                }
                else
                {
                    playerCloseToAttack = false;
                }
            }
            else
            {
                playerCloseToAttack = false;
            }
            return playerCloseToAttack;
        }
        public bool AttackPlayer()
        {
            if (Vector2.SqrMagnitude(Target.position - _fromPos.position) > _config.minSqrDistanceToTarget) return false;
            else return true;
        }
        public bool CheckIfGrounded()
        {
            Vector3 _groundCheck = new Vector3(XDir, -1, 0);
            bool _isGrounded = Physics2D.Raycast(_fromPos.position, _groundCheck, 3.0f, _config.obstructionMask);
            Debug.DrawRay(_fromPos.position, _groundCheck, Color.green);
            return _isGrounded;
        }
        public Vector2 CalculateVelocity(Vector2 fromPosition)
        {
            var sqrDistance = Vector2.SqrMagnitude((Vector2)_pointTarget.position - fromPosition);
            if (sqrDistance <= _config.minSqrDistanceToTarget)
            {
                _pointTarget = GetNextWaypoint();
            }
            var direction = ((Vector2)_pointTarget.position - fromPosition).normalized;
            return _config.speed * direction;
        }
        private Transform GetNextWaypoint()
        {
            _currentPointIndex = (_currentPointIndex + 1) % _config.waypoints.Length;
            return _config.waypoints[_currentPointIndex];
        }
        public Vector2 CalculateChaseVelocity(Vector2 fromPosition)
        {
            var direction = ((Vector2)Target.position - fromPosition).normalized;
            return _config.speed * direction;
        }
        public Vector2 GoToInitialPoint(Vector2 fromPosition, Vector2 startPos)
        {
            var direction = ((Vector2)startPos - fromPosition).normalized;
            return _config.speed * direction;
        }
    }
}
