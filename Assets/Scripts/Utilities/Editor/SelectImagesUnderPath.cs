using UnityEditor;
using UnityEngine;

namespace JTUtility.Editor
{
    public class SelectImagesUnderPath : EditorWindow
    {
        private string path = "";

        [MenuItem("Search Helper/Select Images Under Path")]
        private static void Init()
        {
            GetWindow<SelectImagesUnderPath>().Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            IncreaseIndent();
            GUILayout.BeginHorizontal();
            path = EditorGUILayout.TagField("Path: ", path);

            if (GUILayout.Button("Select"))
            {
                Selection.objects = GameObject.FindGameObjectsWithTag(path);
            }
            GUILayout.EndHorizontal();
            DecreaseIndent();
            GUILayout.EndVertical();
        }

        private void IncreaseIndent()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
        }

        private void DecreaseIndent()
        {
            GUILayout.EndHorizontal();
        }
    }
}