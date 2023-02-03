using UnityEngine;
namespace PlatformerMVC
{
    public class BullView : LevelObjectView
    {
        private int _damagePoint = 10;
        public int DamagePoint { get => _damagePoint; set => _damagePoint = value; }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && !collision.gameObject.GetComponent<LevelObjectView>()
                                  && !collision.gameObject.GetComponentInParent<PlayerView>())
            {
                gameObject.SetActive(false);
            }
        }
    }
}
