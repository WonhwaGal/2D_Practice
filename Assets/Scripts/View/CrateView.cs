using UnityEngine;

namespace PlatformerMVC
{
    public class CrateView : LevelObjectView
    {
        public GameObject _childObject;
        public bool _throwToRight;
        private Vector2 _shift;
        bool _StarThrown = false;
        private void Start()
        {
            int _dir = _throwToRight ? -1: 1;
            _childObject.SetActive(false);
            _shift = new Vector2(-0.5f * _dir, 1.0f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_childObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D child) && !_StarThrown)
            {
                _childObject.SetActive(true);
                child.AddForce((Vector2.up + _shift) * 5, ForceMode2D.Impulse);
                _StarThrown = true;
            }
        }
        private void Update()
        {
            if (Vector3.Distance(gameObject.transform.position, _childObject.transform.position) > 2.0f)
            {
                _childObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D _boxColl);
                _boxColl.enabled = true;
            }
            else
            {
                _childObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D _boxColl);
                _boxColl.enabled = false;
            }
        }
    }
}
