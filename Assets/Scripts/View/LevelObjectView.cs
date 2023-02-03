using UnityEngine;

namespace PlatformerMVC
{
    public class LevelObjectView : MonoBehaviour   // базовая вьюшка для всех на сцене
    {
        public Transform _transform;
        public SpriteRenderer _spriteRenderer;
        public Collider2D _collider;
        public Rigidbody2D _rb;
    }
}
