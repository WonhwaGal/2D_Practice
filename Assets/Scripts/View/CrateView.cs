using UnityEngine;

namespace PlatformerMVC
{
    public class CrateView : LevelObjectView
    {
        [SerializeField] private GameObject _childObject;
        private Collider2D _childCollider;
        private Rigidbody2D _childRb;

        public bool _throwToRight;
        private Vector2 _shift;
        bool _StarThrown = false;
        private void Start()
        {
            int _dir = _throwToRight ? -1: 1;
            _childObject.SetActive(false);
            _shift = new Vector2(-0.5f * _dir, 1.0f);
            _childCollider = _childObject.GetComponent<Collider2D>();
            _childRb = _childObject.GetComponent<Rigidbody2D>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_StarThrown)
            {
                _childObject.SetActive(true);
                _childRb.AddForce((Vector2.up + _shift) * 5, ForceMode2D.Impulse);
                _StarThrown = true;
            }
        }
        private void Update()
        {
            if (Vector3.Distance(gameObject.transform.position, _childObject.transform.position) > 2.0f)
            {
                _childCollider.enabled = true;
            }
            else
            {
                _childCollider.enabled = false;
            }
        }
    }
}
