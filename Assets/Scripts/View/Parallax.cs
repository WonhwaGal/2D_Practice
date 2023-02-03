using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private List<GameObject> parallaxObjects;
        [SerializeField] private List<float> _parallaxEffects;
        private List<ParallaxConfig> _parallaxConfigs = new List<ParallaxConfig>();

        private GameObject cam;

        private void Awake()
        {
            cam = Camera.main.gameObject;

            for (int i = 0; i < parallaxObjects.Count; i++)
            {
                int index = i;
                _parallaxConfigs.Add((ParallaxConfig)ScriptableObject.CreateInstance("ParallaxConfig"));
                _parallaxConfigs[index]._object = parallaxObjects[index];
                _parallaxConfigs[index]._parallaxEffect = _parallaxEffects[index];
                _parallaxConfigs[index]._startPosX = parallaxObjects[index].transform.position.x;
                _parallaxConfigs[index]._length = parallaxObjects[index].GetComponent<SpriteRenderer>().bounds.size.x;
                _parallaxConfigs[index]._shiftX = 0;
            }
        }
        private void Update()
        {
            foreach (var pCfg in _parallaxConfigs)
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
