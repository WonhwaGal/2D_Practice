using UnityEngine;
using System;
using System.Collections.Generic;

namespace PlatformerMVC
{
    public class SpriteAnimationController : IDisposable
    {
        public Action<SpriteRenderer, AnimState> onCancelAnimState;
        private sealed class Animation   // класс анимационного клипа
        {
            public AnimState Track;
            public List<Sprite> Sprites;
            public bool Loop;
            public float Speed = 1;
            public float Counter = 0;
            public bool Sleep;
            public void Update()
            {
                if (Sleep) return; 
                Counter += Time.deltaTime * Speed;
                if (Loop)
                {
                    while(Counter >= Sprites.Count)
                    {
                        Counter -= Sprites.Count;
                    }
                }
                else if (Counter >= Sprites.Count)
                {
                    Counter = Sprites.Count;
                    Sleep = true;
                }
            }
        }
        private AnimationConfig _config;
        private Dictionary<SpriteRenderer, Animation> _activeAnimations = new Dictionary<SpriteRenderer, Animation>();
        public SpriteAnimationController(AnimationConfig _config)
        {
            this._config = _config;
        }
        public void AddandStartAnimation(SpriteRenderer spriteRenderer, AnimState track, bool loop, float speed)
        {
            if (_activeAnimations.TryGetValue(spriteRenderer, out var animation))
            {
                animation.Loop = loop;
                animation.Speed = speed;
                animation.Sleep = false;
                if (animation.Track != track)
                {
                    animation.Track = track;
                    animation.Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites;
                    animation.Counter = 0;
                }
            }
            else
            {
                _activeAnimations.Add(spriteRenderer, new Animation()
                {
                    Track = track,
                    Sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites,
                    Loop = loop,
                    Speed = speed,
                });
            }
        }
        public void StopAnimation(SpriteRenderer _renderer)
        {
            if (_activeAnimations.ContainsKey(_renderer))
            {
                _activeAnimations.Remove(_renderer);
            }
        }
        public void Update()
        {
            foreach (var animation in _activeAnimations)
            {
                if (animation.Value.Sleep)                  // ANIMATION PLAYED OFF
                {
                    onCancelAnimState?.Invoke(animation.Key, animation.Value.Track);
                }

                animation.Value.Update();

                if (animation.Value.Counter < animation.Value.Sprites.Count)
                {
                    animation.Key.sprite = animation.Value.Sprites[(int)animation.Value.Counter];
                }
            }
        }

        public void Dispose()
        {
            _activeAnimations.Clear();
        }
    }
}

