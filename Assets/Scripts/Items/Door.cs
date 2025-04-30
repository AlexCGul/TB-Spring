using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ToggleOpen()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        BoxCollider collider = GetComponent<BoxCollider>();
        
        renderer.enabled = !renderer.enabled;
        collider.enabled = !collider.enabled;
        isOpen = !isOpen;
    }
}
