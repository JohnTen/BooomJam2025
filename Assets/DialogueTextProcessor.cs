#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueTextProcessor : MonoBehaviour
{
    void Start()
    {
        Dictionary<string, string> replacements = new Dictionary<string, string>();
        foreach (var textModel in TextDatabase.Instance.ItemDict)
        {
            replacements.Add($"\"{textModel.Value.lnStrings[TextDatabase.Language.scn]}\"", $"\"{textModel.Value.id}\"");
        }
        ProcessDialogueFile("Assets/Scripts/Dialogue/DialogueEntry.cs", replacements);
    }

    public static void ProcessDialogueFile(string filePath, Dictionary<string, string> replacements)
    {
        // 备份文件
        string backupPath = filePath + ".backup";
        File.Copy(filePath, backupPath, true);

        try
        {
            // 读取并处理文件
            string[] lines = File.ReadAllLines(filePath);
            bool lastLineIsString = false;
            for (int i = 0; i < lines.Length; i++)
            {
                var isString = lines[i].TrimStart().StartsWith("\"");
                if (!isString || !lastLineIsString)
                {
                    lastLineIsString = isString;
                    continue;
                }
                
                lastLineIsString = isString;
                foreach (var replacement in replacements)
                {
                    if (lines[i].Contains(replacement.Key))
                    {
                        lines[i] = lines[i].Replace(replacement.Key, replacement.Value);
                        lines[i] += "//" + replacement.Key;
                    }
                }
            }

            // 写回文件
            File.WriteAllLines(filePath, lines);
            
            // 刷新 AssetDatabase
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"处理文件时发生错误: {e.Message}");
            // 恢复备份
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, filePath, true);
            }
        }
    }
}
#endif