using UnityEngine;

namespace PlatformerMVC
{
    public class QuestCoinModel: IQuestModel
    {
        public bool TryComplete(GameObject actor)
        {
            return actor.CompareTag("QuestCoin");
        }
    }
}
