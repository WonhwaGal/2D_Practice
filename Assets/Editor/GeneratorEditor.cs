using UnityEditor;
using UnityEngine;

namespace PlatformerMVC
{
    [CustomEditor(typeof(GeneratorLevelView))]
    public class GeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GeneratorLevelView _viewScript = (GeneratorLevelView)target;
            GUILayout.Space(7);
            GUILayout.Label("Draw MarshingSquares");
            GeneratorController _controller = new GeneratorController(_viewScript);
            if (GUI.Button(new Rect(150, 345, 120, 20), "Redo"))
            {
                _viewScript._tilemap.ClearAllTiles();
                _controller.Start();
            }
            GUILayout.Space(7);
            GUILayout.Label("Use decorated tiles");
            if (GUI.Button(new Rect(150, 375, 120, 20), "Decorate"))
            {
                _controller.UseDecoratedTiles();
            }
            GUILayout.Space(7);
            GUILayout.Label("Clear all tiles");
            if (GUI.Button(new Rect(150, 405, 120, 20), "Clear tile map"))
            {
                _controller.Clear();
            }
        }
    }
}
