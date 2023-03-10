using System;
using System.Linq;
using System.Collections.Generic;

namespace PlatformerMVC
{
    public class QuestStoryController : IQuestStory
    {
        public Action<int> ProgresReport { get; set; }
        private readonly List<IQuest> _questCollection;
        public bool IsDone => _questCollection.All(value => value.IsCompleted);

        public QuestStoryController(List<IQuest> questCollection)
        {
            _questCollection = questCollection;
            foreach (IQuest quest in _questCollection)
            {
                quest.QuestCompleted += OnQuestCompleted;
            }
            //Reset(0);
            for (int i = 0; i < _questCollection.Count; i++)
            {
                Reset(i);
            }
        }
        private void Reset(int index)
        {
            if (index < 0 || index >= _questCollection.Count)
            {
                return;
            }
            IQuest quest = _questCollection[index];
            if (quest.IsCompleted)
            {
                OnQuestCompleted(this, quest);
            }
            else
            {
                quest.Reset();
            }
        }
        private void FullReset(int index)
        {
            if (index < 0 || index >= _questCollection.Count)
            {
                return;
            }
            _questCollection[index].Reset();
        }
        private void OnQuestCompleted(object sender, IQuest quest)
        {
            int index = _questCollection.IndexOf(quest);
            if (index != _questCollection.IndexOf(_questCollection[^1])) 
            {
                FullReset(_questCollection.IndexOf(_questCollection[^1]));
            } 
            if (IsDone)
            {
                ProgresReport?.Invoke(_questCollection.IndexOf(_questCollection[^1]));
            }
            else
            {
                ProgresReport?.Invoke(index);
            }
        }
        public void Dispose()
        {
            foreach (IQuest quest in _questCollection)
            {
                quest.QuestCompleted -= OnQuestCompleted;
                quest.Dispose();
            }
        }
    }
}
