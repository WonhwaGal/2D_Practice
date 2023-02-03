using UnityEngine;

namespace PlatformerMVC
{
    public class PlatformView : LevelObjectView
    {
        public Transform[] points = new Transform[2];
        public int _index = 1;
        public float _speed;

        private void OnCollisionStay2D(Collision2D collision)
        {
            collision.gameObject.transform.SetParent(transform);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
