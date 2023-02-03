using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class EmitterController
    {
        private List<BullController> bulletControllers = new List<BullController>();
        private Transform _tr;

        private int _index;
        private float _timeTillNextBull;
        private float _startSpeed = 8;
        private float _delay = 5;
        public EmitterController(List<BullView> bulletViews, Transform emitterTr)
        {
            _tr = emitterTr;
            foreach (var bullView in bulletViews)
            {
                bulletControllers.Add(new BullController(bullView));
            }
        }

        public void Update()
        {
            if (_timeTillNextBull > 0)
            {
                bulletControllers[_index].Active(false);
                _timeTillNextBull -= Time.deltaTime;
            }
            else
            {
                _timeTillNextBull = _delay;
                bulletControllers[_index].Throw(_tr.position, -_tr.up * _startSpeed);
                _index++;
                if (_index >= bulletControllers.Count)
                {
                    _index = 0;
                }
            }
        }
    }
}
