using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class CoinsManager : IDisposable
    {
        private const float _animationsSpeed = 10;
        private readonly PlayerView _characterView;
        private SpriteAnimationController _spriteAnimator;
        private readonly List<LevelObjectView> _coinViews;
        private AnimationConfig _coinConfig;

        private AudioSource _audioSource;
        private AudioClip _audioClip;

        public CoinsManager(PlayerView characterView, List<LevelObjectView> coinViews)
        {
            _characterView = characterView;
            _coinViews = coinViews;
            _coinConfig = Resources.Load<AnimationConfig>("CoinAnimation");
            _spriteAnimator = new SpriteAnimationController(_coinConfig);
            _characterView.OnLevelObjectContact += OnLevelObjectContact;
            foreach (var coinView in coinViews)
            {
                _spriteAnimator.AddandStartAnimation(coinView._spriteRenderer, AnimState.Idle, true, _animationsSpeed);
            }
            if (characterView.TryGetComponent(out _audioSource)) _audioClip = Resources.Load<AudioClip>("coin");
        }
        private void OnLevelObjectContact(LevelObjectView contactView)
        {
            if (_coinViews.Contains(contactView))
            {
                _audioSource.PlayOneShot(_audioClip);
                _spriteAnimator.StopAnimation(contactView._spriteRenderer);
                contactView.gameObject.SetActive(false);
            }
        }
        public void Update()
        {
            _spriteAnimator.Update();
        }

        public void Dispose()
        {
            _characterView.OnLevelObjectContact -= OnLevelObjectContact;
        }
    }
}
