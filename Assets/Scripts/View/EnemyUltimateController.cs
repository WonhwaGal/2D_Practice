using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformerMVC
{
    public class EnemyUltimateController : MonoBehaviour
    {
        [SerializeField] private List<EnemyView> _enemyViews;
        [SerializeField] private List<AIConfig> _configs;

        private List<EnemyModel> _enemyModels = new List<EnemyModel>();
        private List<EnemyAI> _enemyAIs = new List<EnemyAI>();
        
        private PlayerView _playerView;
        private EnemyAnimator _animator;

        private AudioSource _audioSource;
        private AudioClip _audioClip;
        private bool _playSound = true;
        void Start()
        {
            _playerView = FindObjectOfType<PlayerView>();
            int _enemyNumber = _enemyViews.Count;
            for (int i = 0; i < _enemyNumber; i++)
            {
                _enemyModels.Add(new EnemyModel(_enemyViews[i].transform, _playerView.transform, _configs[i]));
                _enemyAIs.Add(new EnemyAI(_enemyViews[i], _enemyModels[i]));
            }
            _animator = new EnemyAnimator(_enemyViews);
            _animator.OnPlayingOffAnimation += CancelAnimation;

            if (gameObject.TryGetComponent(out _audioSource)) _audioClip = Resources.Load<AudioClip>("monster");
        }
        private void Update()
        {
            _animator.Update();
        }
        void FixedUpdate()
        {
            foreach (EnemyAI ai in _enemyAIs)
            {
                ai.FixedUpdate();
                if (ai._attackMode && !ai._dieMode)
                {
                    if (!_audioSource.isPlaying && _playSound)
                    {
                        _playSound = false;
                        _audioSource.PlayOneShot(_audioClip);
                    } 
                    _animator.ChangeAnimState(ai.ChangeRenderer, AnimState.Attack);
                }
                else if (ai._idleMode && !ai._dieMode)
                {
                    _animator.ChangeAnimState(ai.ChangeRenderer, AnimState.Idle);
                }
                else if (ai._dieMode)
                {
                    _animator.ChangeAnimState(ai.ChangeRenderer, AnimState.Dead);
                }
            }
        }
        private void CancelAnimation(SpriteRenderer spriteRenderer, AnimState state)
        {
            foreach (EnemyAI ai in _enemyAIs)
            {
                if(ai.ChangeRenderer == spriteRenderer)
                {
                    if (state == AnimState.Attack)
                    {
                        _playSound = true;
                        ai._attackMode = false;
                    }
                }
            }
        }
        private void OnDisable()
        {
            _animator.OnPlayingOffAnimation -= CancelAnimation;
            _animator.Dispose();
            foreach (EnemyAI ai in _enemyAIs)
            {
                ai.Dispose();
            }
        }
    }
}
