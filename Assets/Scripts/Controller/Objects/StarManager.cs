using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class StarManager : IDisposable
    {
        private const float _animSpeed = 10f;
        private readonly PlayerView _playerView;
        private SpriteAnimationController _spriteAnimator;
        private readonly List<LevelObjectView> _starViews;
        private AnimationConfig _starConfig;

        private AudioSource _audioSource;
        private AudioClip _audioClip;
        public StarManager(PlayerView _playerView, List<LevelObjectView> _starViews)
        {
            this._playerView = _playerView;
            this._starViews = _starViews;
            _starConfig = Resources.Load<AnimationConfig>("StarAnimationCfg");
            _spriteAnimator = new SpriteAnimationController(_starConfig);
            foreach(LevelObjectView view in _starViews)
            {
                _spriteAnimator.AddandStartAnimation(view._spriteRenderer, AnimState.Idle, true, _animSpeed);
            }
            if (_playerView.TryGetComponent(out _audioSource)) _audioClip = Resources.Load<AudioClip>("star");

            _playerView.OnLevelObjectContact += OnLevelObjectContact;
        }
        private void OnLevelObjectContact(LevelObjectView _starView)
        {
            if (_starViews.Contains(_starView))
            {
                _audioSource.PlayOneShot(_audioClip);
                _spriteAnimator.StopAnimation(_starView._spriteRenderer);
            }
        }
        public void Update()
        {
            _spriteAnimator.Update();
        }
        public void Dispose()
        {
            _playerView.OnLevelObjectContact -= OnLevelObjectContact;
        }
    }
}
