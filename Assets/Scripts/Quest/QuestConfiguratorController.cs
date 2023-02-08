using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    public class QuestConfiguratorController : IDisposable
    {
        private readonly QuestObjectView _singleQuestView;
        private QuestController _singleQuestController;
        private readonly QuestStoryConfig[] _questStoryConfig;
        private readonly QuestObjectView[] _storyQuestViews;
        private readonly QuestStarModel _questStarModel;
        private readonly QuestCoinModel _questCoinModel;

        private List<IQuestStory> _questStoryList;
        private readonly PlayerView _playerView;
        private readonly UIController _uiManager;
        private Dictionary<QuestType, Func<IQuestModel>> _questFactory = new Dictionary<QuestType, Func<IQuestModel>>(10);
        private Dictionary<StoryType, Func<List<IQuest>, IQuestStory>> _storyFactory = new Dictionary<StoryType, Func<List<IQuest>, IQuestStory>>(10);

        public QuestConfiguratorController(QuestView questView, PlayerView player, UIController UImanager = null)
        {
            if (questView._singleQuest)
            {
                _singleQuestView = questView._singleQuest;
            }
            _questStoryConfig = questView._storyConfig;
            _storyQuestViews = questView._questObjects;
            _questStarModel = new QuestStarModel();
            _questCoinModel = new QuestCoinModel();
            _playerView = player;
            _uiManager = UImanager;
        }
        public void Start()
        {
            if (_singleQuestView != null)
            {
                _singleQuestController = new QuestController(_playerView, _singleQuestView, _questCoinModel);
                _singleQuestController.Reset();
            }

            _questFactory.Add(QuestType.Stars, () => new QuestStarModel());
            _storyFactory.Add(StoryType.Common, questCollection => new QuestStoryController(questCollection));

            _questStoryList = new List<IQuestStory>();
            foreach (QuestStoryConfig cfg in _questStoryConfig)
            {
                _questStoryList.Add(CreateQuestStory(cfg));
            }
            foreach(QuestStoryController storyCont in _questStoryList)
            {
                storyCont.ProgresReport += _uiManager.AddStar;
            }

        }
        private IQuest CreateQuest(QuestConfig cfg)
        {
            int questID = cfg.id;
            QuestObjectView qView = _storyQuestViews.FirstOrDefault(value => value._id == cfg.id);
            if (qView == null)
            {
                Debug.Log("No view");
                return null;
            }
            if (_questFactory.TryGetValue(cfg.type, out var factory))
            {
                IQuestModel qModel = factory.Invoke();
                return new QuestController(_playerView, qView, qModel);
            }
            Debug.Log("No Model");
            return null;
        }
        private IQuestStory CreateQuestStory(QuestStoryConfig cfg)
        {
            List<IQuest> quests = new List<IQuest>();
            foreach(QuestConfig item in cfg.questsConfig)
            {
                IQuest quest = CreateQuest(item);
                if (quest == null) continue;
                quests.Add(quest);
            }
            return _storyFactory[cfg.StoryType].Invoke(quests);        
        }
        public void Dispose()
        {
            foreach (QuestStoryController storyCont in _questStoryList)
            {
                storyCont.ProgresReport -= _uiManager.AddStar;
            }
        }
    }
}
