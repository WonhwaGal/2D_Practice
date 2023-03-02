using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class Main_Second : MonoBehaviour
    {
        [SerializeField] private Transform _playerPos;
        [SerializeField] private CannonView _cannonView;
        [SerializeField] private List<LevelObjectView> _coinViews;
        [SerializeField] private List<LevelObjectView> _deathZoneViews;
        [SerializeField] private List<LevelObjectView> _winZoneViews;
        [SerializeField] private List<LevelObjectView> _flagViews;
        [SerializeField] private List<PlatformView> _platformViews;
        [SerializeField] private List<RestartObjView> _restartingObjs;
        [SerializeField] private List<RestartObjView> _fullReloadObjs;
        [SerializeField] private GameObject _finishPlatform;

        [SerializeField] private QuestView _questView;

        private PlayerView _playerView;
        private PlayerRbController _playerController;

        public float _shootingDistance = 10;
        private CannonController _cannonController;
        private EmitterController _emitterController;

        private CoinsManager _coinsManager;
        private LevelCompleteManager _levelCompleteManager;
        private StarManager _starManager;

        private PlatformManager _platformManager;
        private FlagManager _flagManager;
        private CamManager _camManager;

        private UIController _UIController;
        private QuestConfiguratorController _questConfiguratorController;

        private void Awake()
        {
            _playerView = Instantiate(Resources.Load<PlayerView>("Player"), _playerPos.position, Quaternion.identity);
            _playerController = new PlayerRbController(_playerView);

            _cannonController = new CannonController(_cannonView._muzzleT, _playerView._transform);
            _emitterController = new EmitterController(_cannonView._bullets, _cannonView._emitterT);

            _coinsManager = new CoinsManager(_playerView, _coinViews);
            List<LevelObjectView> _starViews = _winZoneViews.GetRange(0, _winZoneViews.Count - 1);
            _starManager = new StarManager(_playerView, _starViews);
            _levelCompleteManager = new LevelCompleteManager(_playerView, _deathZoneViews, _restartingObjs, _fullReloadObjs);

            _platformManager = new PlatformManager(_platformViews);
            _flagManager = new FlagManager(_playerView, _flagViews);
            _finishPlatform.SetActive(false);

            _camManager = new CamManager(_playerView, 5.5f, 0.0f, 132.0f, 19.0f, 2.5f, 4.0f);

            UIView canvas = Instantiate(Resources.Load<UIView>("Canvas"));
            _UIController = new UIController(_playerController, _playerView, canvas);
            _UIController.OnCollectingAllStars += PutFinishPlatform;
            _questConfiguratorController = new QuestConfiguratorController(_questView, _playerView, _UIController);
            _questConfiguratorController.Start();
        }
        private void Update()
        {
            _playerController.Update();
            _camManager.Update();

            _cannonController.Update();
            if (Vector3.Distance(_playerView._transform.position, _cannonView._emitterT.position) < _shootingDistance)
            {
                _emitterController.Update();
            }

            _coinsManager.Update();
            _starManager.Update();
            _platformManager.Update();

            _UIController.Update();
        }
        private void PutFinishPlatform()
        {
            _finishPlatform.SetActive(true);
        }
        private void OnDisable()
        {
            _playerController.Dispose();
            _coinsManager.Dispose();
            _starManager.Dispose();
            _levelCompleteManager.Dispose();
            _flagManager.Dispose();
            _UIController.Dispose();
            _questConfiguratorController.Dispose();
        }
    }
}
