using System;
using UnityEngine;

namespace PlatformerMVC
{
    public class EnemyView : LevelObjectView
    {
        public Action onGettingHurt;

        private bool _isAlive = true;
        public Vector3 _leftScale;
        public Vector3 _rightScale;
        public Vector3 _lastScale;
        public bool _initialTurnToRight = true;

        private Collider2D _attackCollider;
        private bool _canHurtPlayer;
        private Collider2D[] _boxContacts = new Collider2D[2];
        public Collider2D AttackCollider { get => _attackCollider; set => _attackCollider = value; }
        public bool IsAlive { get => _isAlive; set => _isAlive = value; }

        private void Awake()
        {
            AttackCollider = transform.GetChild(0).GetComponent<Collider2D>();
            AttackCollider.enabled = false;
            _leftScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _rightScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (_initialTurnToRight)
            {
                _lastScale = _rightScale;
            }
            else
            {
                _lastScale = _leftScale;
            }
            bool rb = TryGetComponent(out _rb);
            bool _col = TryGetComponent(out _collider);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerView playerView))
            {
                if (playerView.transform.position.y < transform.position.y) _canHurtPlayer = true;
                else _canHurtPlayer = false;
                if (collision != null && _canHurtPlayer && AttackCollider.enabled)
                {
                    playerView.onGettingPancaked?.Invoke();
                }
            }
            else if (collision != null && collision.gameObject.layer == 9)
            {
                collision.GetContacts(_boxContacts);
                foreach (var contact in _boxContacts)
                {
                    if (contact != null && contact.name == "Enemy")
                    {
                        _attackCollider.isTrigger = false;
                        _attackCollider.enabled = true;
                        _collider.enabled = false;
                        onGettingHurt?.Invoke();
                    }
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision != null && collision.gameObject.layer == 9)
            {
                collision.gameObject.layer = 4;
            }
        }
    }
}
