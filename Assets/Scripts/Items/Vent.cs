using System;
using FMODUnity;
using UnityEngine;

public class Vent : MonoBehaviour, IInteractable
{
    [SerializeField, Tooltip("How much force to use when flying forward")] 
    private float popForce;

    [SerializeField] private EventReference ventUse;

    [SerializeField] private EventReference ventClang;

    private FMODUnity.StudioEventEmitter ventEmitter;
    
    private bool wasActivated = false;

    private Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ventEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
        rb.isKinematic = true;
    }

    
    public void Interact()
    {
        if (!rb)
        {
            Debug.Log("No rigidbody found on: " + gameObject.name);
            return;
        }
        
        wasActivated = true;
        rb.isKinematic = false;
        gameObject.layer = 8; // Set to unhittable
        rb.AddForce(transform.forward * popForce, ForceMode.Impulse);
        FMODUnity.RuntimeManager.PlayOneShot(ventUse, transform.position);
        ventEmitter.Stop();
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


    private void OnCollisionEnter(Collision other)
    {
        FMODUnity.RuntimeManager.PlayOneShot(ventClang, transform.position);

    }
}
