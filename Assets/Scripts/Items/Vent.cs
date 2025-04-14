using UnityEngine;

public class Vent : MonoBehaviour, IInteractable
{
    [SerializeField, Tooltip("How much force to use when flying forward")] 
    private float popForce;

    private bool wasActivated = false;

    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
    }

    
    public void Interact()
    {
        if (!rb)
        {
            Debug.Log("No rigidbody found on: " + gameObject.name);
            return;
        }
        
        rb.isKinematic = false;
        rb.AddForce(transform.forward * popForce, ForceMode.Impulse);
    }

    public bool IsInteractable(GameObject interactingCharacter)
    {
        // don't allow the vent to be reactivated
        if (wasActivated)
            return false;
        
        // check if the character is behind the vent
        Vector3 directionToVent = (interactingCharacter.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward * -1, directionToVent);
        if (angle < 90)
        {
            return true;
        }

        return false;
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
}
