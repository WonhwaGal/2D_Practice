using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class ParallaxObject
    {
        public GameObject _object;
        public float _parallaxEffect;
        public float _startPosX;
        public float _length;
        public float _shiftX;

        public ParallaxObject(GameObject obj, float parallaxEffect, float startPosX, float length, float shiftX)
        {
            _object = obj;
            _parallaxEffect = parallaxEffect;
            _startPosX = startPosX;
            _length = length;
            _shiftX = shiftX;
        }
    }
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private List<GameObject> parallaxObjects;
        [SerializeField] private List<float> _parallaxEffects;
        private List<ParallaxObject> _parallaxObjs = new List<ParallaxObject>();

        private GameObject cam;

        private void Awake()
        {
            cam = Camera.main.gameObject;

            for (int i = 0; i < parallaxObjects.Count; i++)
            {
                int index = i;
                float Xpos = parallaxObjects[index].transform.position.x;
                float leng = parallaxObjects[index].GetComponent<SpriteRenderer>().bounds.size.x;
                _parallaxObjs.Add(new ParallaxObject(parallaxObjects[index], _parallaxEffects[index], Xpos, leng, 0));
            }
        }
        private void Update()
        {
            foreach (var pCfg in _parallaxObjs)
            {
                float temp = cam.transform.position.x * (1 - pCfg._parallaxEffect);

                pCfg._shiftX = cam.transform.position.x * pCfg._parallaxEffect;
                pCfg._object.transform.position = new Vector3(pCfg._startPosX + pCfg._shiftX, pCfg._object.transform.position.y, 0);

                if (temp > pCfg._startPosX + pCfg._length)
                {
                    pCfg._startPosX += pCfg._length;
                }
                else if (temp < pCfg._startPosX - pCfg._length)
                {
                    pCfg._startPosX -= pCfg._length;
                }
            }
        }
    }
}
