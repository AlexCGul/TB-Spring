using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent<Pickup> onPickup;
    [SerializeField] public Texture2D sprite;
    public bool isHeld = false;

    [HideInInspector] public Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Inventory inventory = PlayerController.Instance.GetComponent<Inventory>();
        onPickup.AddListener(inventory.PickupItem);
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        onPickup?.Invoke(this);
        isHeld = true;
    }

    public bool IsInteractable(GameObject interactingCharacter)
    {
        return !isHeld;
    }

    public string GetInteractionType()
    {
        return "Pickup";
    }

    public GameObject GetOwner()
    {
        if (!this)
            return null;
        return gameObject;
    }

    public float GetInteractionDistanceMultiplier()
    {
        return 1;
    }
}
