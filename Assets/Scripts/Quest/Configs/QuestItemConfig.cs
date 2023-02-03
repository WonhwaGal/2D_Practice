using System.Collections.Generic;
using UnityEngine;

namespace PlatformerMVC
{
    [CreateAssetMenu(fileName = "QuestItemCfg", menuName = "Configs/Quest System/Quest Item Cfg")]
    public class QuestItemConfig : ScriptableObject
    {
        public int questId;
        public List<int> questItemID;
    }
}
