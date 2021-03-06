#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

public class SettingsClear : EditorWindow
{
    [MenuItem("Edit/Saves/Clear All")]
    public static void ClearAll()
    {
        string path = Application.dataPath + "/Saves";
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        else
            Debug.LogError("The folder does not exist.");
    }

    [MenuItem("Edit/Saves/Clear Language")]
    public static void ClearLabguage()
    {
        string path = Path.Combine(Application.dataPath + "/Saves", "Language.json");
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.LogError("The file does not exist.");
    }

    [MenuItem("Edit/Saves/Clear Control")]
    public static void ClearControl()
    {
        string path = Path.Combine(Application.dataPath + "/Saves", "Control.json");
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.LogError("The file does not exist.");
    }

    [MenuItem("Edit/Saves/Clear Volume")]
    public static void ClearVolume()
    {
        string path = Path.Combine(Application.dataPath + "/Saves", "Volume.json");
        if (File.Exists(path))
            File.Delete(path);
        else
            Debug.LogError("The file does not exist.");
    }
}
#endif