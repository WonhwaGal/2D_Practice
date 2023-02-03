using System;

namespace PlatformerMVC
{
    public class QuestController : IQuest
    {
        private PlayerView _playerView;
        private bool _active;
        private IQuestModel _model;
        private QuestObjectView _questView;

        public event EventHandler<IQuest> QuestCompleted;
        public bool IsCompleted { get; private set; }
        public QuestController(PlayerView player, QuestObjectView questView, IQuestModel model)
        {
            _playerView = player;
            _active = false;
            _model = model;
            _questView = questView;
        }
        public void Reset()
        {
            if (_active) return;
            _active = true;
            IsCompleted = false;
            _playerView.OnQuestComplete += OnContact;
            _questView.ProcessActivate();
        }
        public void OnContact(QuestObjectView questItem)
        {
            if (questItem != null)
            {
                if (_model.TryComplete(questItem.gameObject))
                {
                    if (questItem == _questView)
                    {
                        Completed();
                    }
                }
            }
        }
        public void Completed()
        {
            if (!_active) return;
            _active = false;
            IsCompleted = true;
            _playerView.OnQuestComplete -= OnContact;
            _questView.ProcessComplete();
            QuestCompleted?.Invoke(this, this);
        }
        public void Dispose()
        {
            _playerView.OnQuestComplete -= OnContact;
        }
    }
}
