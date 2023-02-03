using UnityEngine;
using UnityEditor;

namespace PlatformerMVC
{
    [CustomEditor(typeof(ReloadScript))]
    public class RetryScript : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(20);
            if (GUI.Button(new Rect(120, 30, 120, 20), "Reset PlayerPrefs"))
            {
                PlayerPrefs.DeleteKey("Highest");
                PlayerPrefs.DeleteKey("LevelOne");
                PlayerPrefs.DeleteKey("LevelTwo");
                PlayerPrefs.DeleteKey("LevelThree");
            }
            GUILayout.Space(10);
        }
    }
}
