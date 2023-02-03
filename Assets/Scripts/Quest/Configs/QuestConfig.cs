using UnityEngine;

namespace PlatformerMVC 
{ 
    public enum QuestType
    {
        Coins,
        Stars
    }
    [CreateAssetMenu(fileName = "QuestCfg", menuName = "Configs/Quest System/Quest Cfg")]
    public class QuestConfig : ScriptableObject
    {
        public int id;
        public QuestType type;
    }
}
