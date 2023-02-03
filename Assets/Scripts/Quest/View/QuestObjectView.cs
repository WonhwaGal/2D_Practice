using System;
using UnityEngine;

namespace PlatformerMVC
{
    public class QuestObjectView : LevelObjectView
    {
        [SerializeField] private Color _completedColor;
        public int _id;
        public bool _shouldTurnOff = true;
        private Color _defaultColor;
        private void Awake()
        {
            if (_spriteRenderer != null)
            {
                _defaultColor = _spriteRenderer.material.color;
            }
        }
        public void ProcessActivate()
        {
            if (gameObject.CompareTag("QuestCoin"))
            {
                _spriteRenderer.material.color = _defaultColor;
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        public void ProcessComplete()
        {
            if (gameObject.CompareTag("QuestCoin"))
            {
                _spriteRenderer.material.color = _completedColor;
            }
            else
            {
                if (_shouldTurnOff)
                gameObject.SetActive(false);
            }
        }
    }
}
