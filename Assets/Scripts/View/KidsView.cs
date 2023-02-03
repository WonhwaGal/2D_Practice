using UnityEngine;

namespace PlatformerMVC
{
    public class KidsView : LevelObjectView
    {
        public Vector3 _leftScale;
        public Vector3 _rightScale;
        public bool _isPatrol;
        public bool _isBoy;
        public bool _shouldAct;
        private void Awake()
        {
            _leftScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _rightScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        private void OnBecameVisible()
        {
            _shouldAct = true;
        }
        private void OnBecameInvisible()
        {
            _shouldAct = false;
        }
    }
}
