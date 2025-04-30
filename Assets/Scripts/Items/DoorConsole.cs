using FMODUnity;
using UnityEngine;

public class DoorConsole : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isActive = false;
    [SerializeField] private Door connectedDoor;

    [SerializeField] private EventReference activateSound;
    [SerializeField] private EventReference pressedSound;

    [SerializeField] private UnityEngine.Light[] lights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!connectedDoor)
            connectedDoor = GetComponentInParent<Door>();
    }
    

    public void Interact()
    {
        connectedDoor.ToggleOpen();
        FMODUnity.RuntimeManager.PlayOneShot(pressedSound, transform.position);
    }

    public bool IsInteractable(GameObject interactingCharacter)
    {
        return isActive;
    }

    public string GetInteractionType()
    {
        return "Use";
    }

    public GameObject GetOwner()
    {
        return gameObject;
    }

    public float GetInteractionDistanceMultiplier()
    {
        return 1;
    }


    public void ToggleActive()
    {
        isActive = !isActive;
        
        if (lights.Length == 0)
            return;
        
        Color c = isActive ? Color.green : Color.red;
        foreach (UnityEngine.Light l in lights)
        {
            l.color = c;
        }
        
        if (isActive)
            FMODUnity.RuntimeManager.PlayOneShot(activateSound, transform.position);
    }


    public void SetActive()
    {
        isActive = true;
        if (lights.Length == 0)
            return;
        
        foreach (UnityEngine.Light l in lights)
        {
            l.color = Color.green;
        }
        
        FMODUnity.RuntimeManager.PlayOneShot(activateSound, transform.position);
    }
    
    
    public void SetInactive()
    {
        isActive = false;
        
        if (lights.Length == 0)
            return;
        
        foreach (UnityEngine.Light l in lights)
        {
            l.color = Color.red;
        }
    }
}
