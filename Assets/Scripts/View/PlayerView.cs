using UnityEngine;
using System;

namespace PlatformerMVC
{
    public class PlayerView : LevelObjectView
    {
        public Collider2D _headCollider;
        private Vector3 _startPos;

        public Action<LevelObjectView> OnLevelObjectContact { get; set; }
        public Action<BullView> TakeDamage { get; set; }
        public Action<LevelObjectView> OnCollectingCoin { get; set; }
        public Action onGettingHurt { get; set; }
        public Action onGettingPancaked { get; set; }
        public Action onTouchingDeathZone { get; set; }
        public Action<QuestObjectView> OnQuestComplete { get; set; }
        public Vector3 StartPos { get => _startPos; set => _startPos = value; }

        private Collider2D[] _bulletContacts = new Collider2D[2];
        private void Start()
        {
            StartPos = transform.position;
        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out LevelObjectView contactView))
            {
                if (contactView is BullView bullView)
                {
                    collider.GetContacts(_bulletContacts);

                    for (int i = 0; i < _bulletContacts.Length; i++)
                    {
                        if (_bulletContacts[i] != null && _bulletContacts[i].name == "Head")
                        {
                            collider.gameObject.GetComponent<BullView>().DamagePoint = 100;
                        }
                    }
                    TakeDamage?.Invoke(bullView);
                }
                else if (contactView is QuestObjectView)
                {
                    OnQuestComplete?.Invoke((QuestObjectView)contactView);
                }
                else if (contactView is CoinView) OnCollectingCoin?.Invoke(contactView);

                var levelObject = collider.gameObject.GetComponent<LevelObjectView>();
                OnLevelObjectContact?.Invoke(levelObject);

            }
        }
    }
}
