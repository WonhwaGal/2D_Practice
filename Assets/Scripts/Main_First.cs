using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class Main_First : MonoBehaviour
    {
        [SerializeField] private Transform _playerPos;
        [SerializeField] private List<LevelObjectView> _coinViews;
        [SerializeField] private List<LevelObjectView> _deathZoneViews;
        [SerializeField] private List<LevelObjectView> _winZoneViews;
        [SerializeField] private List<LevelObjectView> _flagViews;
        [SerializeField] private List<PlatformView> _platformViews;
        [SerializeField] private List<RestartObjView> _restartingObjs;

        [SerializeField] private QuestObjectView _singleQuestItem;
        [SerializeField] private QuestView _questView;

        private PlayerView _playerView;
        private PlayerRbController _playerController;

        private CoinsManager _coinsManager;
        private LevelCompleteManager _levelCompleteManager;
        private StarManager _starManager;
        private PlatformManager _platformManager;

        private CamManager _camManager;
        private FlagManager _flagManager;
        private UIController _UIController;

        private QuestController _questController;
        private QuestConfiguratorController _questConfiguratorController;
        private void Awake()
        {
            _playerView = Instantiate(Resources.Load<PlayerView>("Player"), _playerPos.position, Quaternion.identity);
            _playerController = new PlayerRbController(_playerView);

            _camManager = new CamManager(_playerView, 5.5f, 0.0f, 160.0f, 19.0f, 3.0f, 4.0f);

            _coinsManager = new CoinsManager(_playerView, _coinViews);

            List<LevelObjectView> _starViews = _winZoneViews.GetRange(0, _winZoneViews.Count - 1);
            _starManager = new StarManager(_playerView, _starViews);
            _levelCompleteManager = new LevelCompleteManager(_playerView, _deathZoneViews, _restartingObjs);
            _platformManager = new PlatformManager(_platformViews);

            _flagManager = new FlagManager(_playerView, _flagViews);
            UIView canvas = Instantiate(Resources.Load<UIView>("Canvas"));
            _UIController = new UIController(_playerController, _playerView, canvas);

            _questController = new QuestController(_playerView, _singleQuestItem, new QuestCoinModel());
            _questController.Reset();
            _questConfiguratorController = new QuestConfiguratorController(_questView, _playerView, _UIController);
            _questConfiguratorController.Start();
        }
        private void Update()
        {
            _playerController.Update();
            _camManager.Update();

            _coinsManager.Update();
            _starManager.Update();
            _platformManager.Update();

            _UIController.Update();
        }
        private void OnDestroy()
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
