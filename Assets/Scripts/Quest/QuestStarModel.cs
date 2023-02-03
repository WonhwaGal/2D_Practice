using UnityEngine;

namespace PlatformerMVC
{
    public class QuestStarModel : IQuestModel
    {
        public bool TryComplete(GameObject actor)
        {
            return actor.CompareTag("QuestStar");
        }
    }
}
