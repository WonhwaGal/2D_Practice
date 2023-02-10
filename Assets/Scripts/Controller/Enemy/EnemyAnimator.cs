using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    enum EnemyAnimSpeed
    {
        IdleSpeed = 4,
        WalkSpeed = 6,
        AttackSpeed = 8,
        DieSpeed = 8,
    }
    public sealed class EnemyAnimator : IDisposable
    {
        public Action<SpriteRenderer, AnimState> OnPlayingOffAnimation { get; set; }

        private readonly List<EnemyView> _levelObjectViews;
        private readonly SpriteAnimationController _animationController;
        private readonly AnimationConfig _config;
        private bool _loop;
        private const float _moveTreshold = 0.2f;

        private bool _delayAction = false;
        private float _delay;
        private SpriteRenderer _spriteRendererInQuestion;

        public EnemyAnimator(List<EnemyView> views)
        {
            _levelObjectViews = views;
            _config = Resources.Load<AnimationConfig>("EnemyAnimCfg");
            _animationController = new SpriteAnimationController(_config);
            foreach (EnemyView view in _levelObjectViews)
            {
                _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.Idle, true, (float)EnemyAnimSpeed.IdleSpeed);
            }
            _animationController.onCancelAnimState += CancelAnimState;
        }

        public void Update()
        {
            _animationController.Update();
            foreach(EnemyView view in _levelObjectViews)
            {
                if (view._spriteRenderer == _spriteRendererInQuestion && _delayAction)
                {
                    if (_delay > 0) _delay -= Time.deltaTime;
                    else
                    {
                        view.AttackCollider.enabled = true;
                        _delayAction = false;
                    }
                }

                if (view._rb.velocity.x != 0 && view.IsAlive)
                {
                    view._transform.localScale = view._rb.velocity.x > 0 ? view._rightScale : view._leftScale;
                    view._lastScale = view._transform.localScale;
                }
                else
                {
                    view._transform.localScale = view._lastScale;
                }

                if (Mathf.Abs(view._rb.velocity.x) > _moveTreshold)
                {
                    _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.Walk, true, (float)EnemyAnimSpeed.WalkSpeed);
                    view.AttackCollider.enabled = false;
                }
            }
        }
        public void ChangeAnimState(SpriteRenderer spriteRenderer, AnimState state)
        {
            foreach (EnemyView view in _levelObjectViews)
            {
                if (view._spriteRenderer == spriteRenderer)
                {
                    float _speed = 0;
                    switch (state)
                    {
                        case AnimState.Walk:
                            _speed = (float)EnemyAnimSpeed.WalkSpeed;
                            _loop = true;
                            break;
                        case AnimState.Idle:
                            _speed = (float)EnemyAnimSpeed.IdleSpeed;
                            _loop = true;
                            break;
                        case AnimState.Attack:
                            _speed = (float)EnemyAnimSpeed.AttackSpeed;
                            _loop = false;
                            break;
                        case AnimState.Dead:
                            _speed = (float)EnemyAnimSpeed.DieSpeed;
                            _loop = false;
                            break;
                    }
                    _animationController.AddandStartAnimation(view._spriteRenderer, state, _loop, _speed);
                    if (state == AnimState.Attack && !_delayAction)
                    {
                        _delayAction = true;
                        _delay = 0.45f;
                        _spriteRendererInQuestion = view._spriteRenderer;
                    }
                }
            }
        }
        public void CancelAnimState(SpriteRenderer spriteRenderer, AnimState state)
        {
            foreach (EnemyView view in _levelObjectViews)
            {
                if (view._spriteRenderer == spriteRenderer && view.IsAlive)
                {
                    OnPlayingOffAnimation?.Invoke(spriteRenderer, AnimState.Attack);
                    _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.Idle, true, (float)EnemyAnimSpeed.IdleSpeed);
                    view.AttackCollider.enabled = false;
                }
            }
        }
        
        public void Dispose()
        {
            _animationController.onCancelAnimState -= CancelAnimState;
        }
    }
}
