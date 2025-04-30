using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class DetectCorruptedAssets : EditorWindow
{
    [MenuItem("Tools/Detect Corrupted Assets")]
    public static void ShowWindow()
    {
        GetWindow<DetectCorruptedAssets>("Detect Corrupted Assets").ScanAssets();
    }

    private void ScanAssets()
    {
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        List<string> failedAssets = new List<string>();

        int checkedCount = 0;

        foreach (string path in allAssetPaths)
        {
            if (!path.StartsWith("Assets")) continue;

            Object obj = AssetDatabase.LoadMainAssetAtPath(path);
            if (obj == null)
            {
                failedAssets.Add(path);
                Debug.LogError($"Failed to load asset at path: {path}");
            }

            checkedCount++;
        }

        EditorUtility.DisplayDialog(
            "Scan Complete",
            $"Checked {checkedCount} assets.\nFailed to load: {failedAssets.Count}",
            "OK"
        );
    }
}