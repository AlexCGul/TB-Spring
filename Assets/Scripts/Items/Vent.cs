using UnityEngine;

public class Vent : MonoBehaviour, IInteractable
{
    [SerializeField, Tooltip("How much force to use when flying forward")] 
    private float popForce;

    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
        Interact(null);

    }

    
    public void Interact(Transform interactingCharacter)
    {
        if (!rb)
        {
            Debug.Log("No rigidbody found on: " + gameObject.name);
            return;
        }
        rb.isKinematic = false;
        rb.AddForce(transform.forward * popForce, ForceMode.Impulse);
    }
    

    public string GetInteractionType()
    {
        return "Use";
    }
}
