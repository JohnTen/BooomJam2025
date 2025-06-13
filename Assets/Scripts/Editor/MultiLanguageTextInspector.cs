using JTUtility;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MultiLanguageText))]
public class MultiLanguageTextInspector : Editor
{
    SerializedProperty textProperty;
    SerializedProperty tmpTextProperty;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (textProperty.IsNull())
            textProperty = serializedObject.FindProperty("legacyText");
        if (tmpTextProperty.IsNull())
            tmpTextProperty = serializedObject.FindProperty("tmpText");

        var database = TextDatabase.Instance;

        var self = target as MultiLanguageText;
        if (self == null)
        {
            return;
        }

        if (textProperty.objectReferenceValue == null && tmpTextProperty.objectReferenceValue == null)
        {
            var text = self.GetComponent<Text>();
            var tmpText = self.GetComponent<TMPro.TMP_Text>();

            if (text == null && tmpText == null)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("No text or tmpText components found on this object");
                EditorGUILayout.EndVertical();
                return;
            }

            if (text != null)
            {
                textProperty.objectReferenceValue = text;
            }
            if (tmpText != null)
            {
                tmpTextProperty.objectReferenceValue = tmpText;
            }

            serializedObject.ApplyModifiedProperties();
        }

        var textid = serializedObject.FindProperty("_textID")?.stringValue;
        if (string.IsNullOrEmpty(textid))
        {
            return;
        }

        TextModel item = null;
        if (database.ContainsID(textid))
        {
            item = database.GetItem(textid);
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space(10);

        if (item != null)
        {
            EditorGUILayout.TextArea(item.lnStrings[TextDatabase.Language.scn]);
            EditorGUILayout.TextArea(item.lnStrings[TextDatabase.Language.en]);
            if (GUILayout.Button("Apply text"))
            {
                self.UpdateText();
            }
        }
        else
        {
            EditorGUILayout.LabelField("Invalid ID");
        }

        EditorGUILayout.EndVertical();
    }
}
