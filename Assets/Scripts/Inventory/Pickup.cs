using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent<Pickup> onPickup;
    [SerializeField] public Texture2D sprite;

    [HideInInspector] public Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        onPickup?.Invoke(this);

        if (InventoryView.iv != null)
        {
            InventoryView.iv.Pickup(this);
            
            #if UNITY_EDITOR
            Debug.Log("Item added to UI: " + gameObject.name);
            #endif
        }
        else
        {
            #if UNITY_EDITOR
            Debug.LogWarning("InventoryView is null, cannot add item to inventory.");
            #endif
        }
        {
            
        }
    }

    public bool IsInteractable(GameObject interactingCharacter)
    {
        return true;
    }

    public string GetInteractionType()
    {
        return "Pickup";
    }

    public GameObject GetOwner()
    {
        return gameObject;
    }

    public float GetInteractionDistanceMultiplier()
    {
        return 1;
    }
}
