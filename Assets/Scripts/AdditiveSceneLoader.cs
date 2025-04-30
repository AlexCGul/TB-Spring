using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;
using UnityEngine;

public class AdditiveSceneLoader : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private string[] sceneNames;
    private void OnValidate()
    {
        int newSceneCount = 0;

        for (int x = 0; x < sceneNames.Length; x++)
        {
            string[] guids = AssetDatabase.FindAssets($"{sceneNames[x]} t:Scene");

            if (guids.Length == 0)
            {
                Debug.LogError($"Scene '{sceneNames[x]}' not found.");
                return;
            }

            string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);

            if (EditorSceneManager.GetSceneByPath(scenePath).isLoaded)
            {
                Debug.Log($"Scene '{sceneNames[x]}' is already loaded.");
                return;
            }

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            Debug.Log($"Scene '{sceneNames[x]}' loaded additively.");
        }
        
        Debug.Log(newSceneCount + " new scenes loaded");
    }


    private void Start()
    {
        for (int x = 0; x < sceneNames.Length; x++)
        {
            if (SceneExists(sceneNames[x]))
            {
                continue;
            }
            SceneManager.LoadScene(sceneNames[x], LoadSceneMode.Additive);
        }
    }


    // Check if scene exists
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        
        return false;
    }
    
    #endif
}
