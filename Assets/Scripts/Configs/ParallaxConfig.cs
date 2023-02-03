using UnityEngine;

namespace PlatformerMVC
{
    [CreateAssetMenu(fileName = "ParallaxCfg", menuName = "Configs / Parallax", order = 2)]
    public class ParallaxConfig : ScriptableObject
    {
        public GameObject _object;
        public float _parallaxEffect;
        public float _startPosX;
        public float _length;
        public float _shiftX;
    }
}
