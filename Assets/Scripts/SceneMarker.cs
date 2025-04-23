using UnityEngine;

public class SceneMarker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #if !UNITY_EDITOR
        Destroy(gameObject);
        #endif
    }
    
}
