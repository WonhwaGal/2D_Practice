using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public enum AnimState
    {
        Idle = 0,
        Walk = 1, 
        Jump = 2,
        Slide = 3,
        Swim = 4,
        Dead = 5,
        GirlWalk = 6,
        BoyWalk = 7,
        GirlIdle = 8,
        BoyIdle = 9,
        Attack =10,
    }
    [CreateAssetMenu(fileName = "SpriteAnimationCfg", menuName = "Configs / Animation", order = 1)]
    public class AnimationConfig : ScriptableObject
    {
        [Serializable]
        public class SpriteSequence
        {
            public AnimState Track;
            public List<Sprite> Sprites = new List<Sprite>();
        }
        public List<SpriteSequence> Sequences = new List<SpriteSequence>();
    }
}

