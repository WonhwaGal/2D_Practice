using UnityEngine;

namespace PlatformerMVC
{
    public enum StoryType
    {
        Common,
        Resetable
    }
    [CreateAssetMenu(fileName = "QuestStoryCfg", menuName = "Configs/Quest System/Quest Story Cfg")]
    public class QuestStoryConfig : ScriptableObject
    {
        public QuestConfig[] questsConfig;
        public StoryType StoryType;
    }
}
