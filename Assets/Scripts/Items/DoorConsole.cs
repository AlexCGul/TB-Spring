using UnityEngine;

public class DoorConsole : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isActive = false;
    private Door connectedDoor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        connectedDoor = GetComponentInParent<Door>();
    }
    

    public void Interact()
    {
        connectedDoor.ToggleOpen();
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
    }


    public void SetActive()
    {
        isActive = true;
    }
    
    
    public void SetInactive()
    {
        isActive = false;
    }
}
