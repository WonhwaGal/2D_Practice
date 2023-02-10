using System;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class EnemyAI : IDisposable
    {
        private readonly EnemyView _enemyView;
        private readonly EnemyModel _model;

        private Vector3 _startPos;
        public bool _attackMode;
        public bool _idleMode;
        public bool _dieMode;
        private SpriteRenderer _changeRenderer;

        public SpriteRenderer ChangeRenderer { get => _changeRenderer; set => _changeRenderer = value; }

        public EnemyAI(EnemyView view, EnemyModel model)
        {
            _enemyView = view;
            _model = model;
            _startPos = view._transform.position;
            ChangeRenderer = _enemyView._spriteRenderer;
            _enemyView.onGettingHurt += Die;
            _dieMode = false;
        } 
        private void Die()
        {
            _dieMode = true;
            _enemyView.IsAlive = false;
            _enemyView._rb.velocity = Vector2.zero;
        }
        public void FixedUpdate()
        {
            if (_model._type == EnemyType.Guard && _enemyView.IsAlive) // GUARD
            {
                if (_model.PlayerFound() && _model.CheckIfGrounded())
                {
                    if (_model.AttackPlayer())
                    {
                        AttackPlayer();
                    }
                    else if ((_model.XDir > 0 && _enemyView._lastScale == _enemyView._rightScale ||
                        _model.XDir < 0 && _enemyView._lastScale == _enemyView._leftScale) && !_attackMode)   // && AttackMode == false
                    {
                        ChasePlayer();
                    }
                    else if (!_attackMode)
                    {
                        GoToInitialPos();
                    }
                }
                else if (!_attackMode)
                {
                    GoToInitialPos();
                }
            }
            else if (_model._type == EnemyType.Patrol && _enemyView.IsAlive)  // PATROL
            {
                if (_model.PlayerFound() && _model.CheckIfGrounded())
                {
                    if (_model.AttackPlayer())
                    {
                        AttackPlayer();
                    }
                    else if ((_model.XDir > 0 && _enemyView._lastScale == _enemyView._rightScale ||
                        _model.XDir < 0 && _enemyView._lastScale == _enemyView._leftScale) && !_attackMode)
                    {
                        ChasePlayer();
                    }
                    else if (!_attackMode)
                    {
                        GoPatrol();
                    }
                }
                else
                {
                    GoPatrol();
                }
            }
        }
        private void GoToInitialPos()
        {
            _enemyView._rb.velocity = _model.GoToInitialPoint(_enemyView.transform.position, _startPos) * Time.fixedDeltaTime;
            _dieMode = false;
            if (Vector2.SqrMagnitude(_startPos - _enemyView.transform.position) < 1.0f)
            {
                _enemyView._rb.velocity = Vector2.zero;
                _idleMode = true;
            }
            else
            {
                _idleMode = false;
            }
        }
        private void GoPatrol()
        {
            _dieMode = false;
            var newVelocity = _model.CalculateVelocity(_enemyView._transform.position) * Time.fixedDeltaTime;
            _enemyView._rb.velocity = newVelocity;
        }
        private void ChasePlayer()
        {
            _idleMode = false;
            _dieMode = false;
            _enemyView._rb.velocity = _model.CalculateChaseVelocity(_enemyView.transform.position) * Time.fixedDeltaTime;
        }
        private void AttackPlayer()
        {
            _attackMode = true;
            _enemyView._rb.velocity = Vector2.zero;
            if (_model.XDir > 0) _enemyView._lastScale = _enemyView._rightScale;
            else _enemyView._lastScale = _enemyView._leftScale;
        }
        public void Dispose()
        {
            _enemyView.onGettingHurt -= Die;
        }
    }
}
