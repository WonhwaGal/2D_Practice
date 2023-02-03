using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class KidsConfigurator : MonoBehaviour
    {
        [SerializeField] private List<AIConfig> _simplePatrolAIConfigs;
        [SerializeField] private List<KidsView> _simplePatrolAIViews;
        [SerializeField] private LayerMask _layerMask;
        #region Fields
        private List<SimplePatrolAI> _simplePatrolAIs = new List<SimplePatrolAI>();
        private Transform _playerT;
        private KidsManager _kidsManager;
        #endregion

        #region Unity methods
        private void Start()
        {
            int numberOfKids = _simplePatrolAIViews.Count;
            _playerT = FindObjectOfType<PlayerView>().transform;
            for (int i = 0; i < numberOfKids; i++)
            {
                if (_simplePatrolAIViews[i]._isPatrol)
                {
                    _simplePatrolAIs.Add(new SimplePatrolAI(_simplePatrolAIViews[i], 
                                         new SimplePatrolAIModel(_simplePatrolAIConfigs[i])));
                }
                else
                {
                    _simplePatrolAIs.Add(new SimplePatrolAI(_simplePatrolAIViews[i], 
                                         new SimpleWaitingAIModel(_simplePatrolAIViews[i]._transform, 
                                         _playerT, _simplePatrolAIConfigs[i], _layerMask)));
                }
            }
            _kidsManager = new KidsManager(_simplePatrolAIViews);
            
        }

        private void Update()
        {
            _kidsManager.Update();
        }
        private void FixedUpdate()
        {
            foreach (SimplePatrolAI aiConfig in _simplePatrolAIs)
            {
                if (aiConfig != null) aiConfig.FixedUpdate();
            }
        }
        #endregion
    }

}
