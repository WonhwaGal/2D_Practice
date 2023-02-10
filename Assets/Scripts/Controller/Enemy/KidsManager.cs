using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public sealed class KidsManager
    {
        private const int speed = 10;
        private const float _threshold = -0.1f;
        private const float _moveTreshold = 0.5f;

        AnimationConfig _config;
        private readonly List<KidsView> _views;
        private SpriteAnimationController _animationController;
        public KidsManager(List<KidsView> _views)
        {
            this._views = _views;
            _config = Resources.Load<AnimationConfig>("KidsAnimation");
            _animationController = new SpriteAnimationController(_config);
            foreach (var view in _views)
            {
                if (view._isPatrol)
                {
                    if (view._isBoy)
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.BoyWalk, true, speed);
                    }
                    else
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.GirlWalk, true, speed);
                    }
                }
                else
                {
                    if (view._isBoy)
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.BoyIdle, true, speed);
                    }
                    else
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.GirlIdle, true, speed);
                    }
                }
            }
        }
        public void Update()
        {
             _animationController.Update();
            foreach (var view in _views)
            {
                view._transform.localScale = view._rb.velocity.x >= _threshold ? view._rightScale : view._leftScale;
                if (!view._isPatrol && Mathf.Abs(view._rb.velocity.x) > _moveTreshold)
                {
                    if (view._isBoy)
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.BoyWalk, true, speed);
                    }
                    else
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.GirlWalk, true, speed);
                    }
                }
                else if (!view._isPatrol && Mathf.Abs(view._rb.velocity.x) <= _moveTreshold)
                {
                    if (view._isBoy)
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.BoyIdle, true, speed);
                    }
                    else
                    {
                        _animationController.AddandStartAnimation(view._spriteRenderer, AnimState.GirlIdle, true, speed);
                    }
                }
            }
        }
    }
}
