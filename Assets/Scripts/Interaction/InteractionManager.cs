using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    List<IInteractable> interactables = new List<IInteractable>();
    private PlayerController player;
    PlayerInput input;
    [SerializeField] private float standardInteractionDistance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get all the interactables
        interactables = new List<IInteractable>(FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>());
        player = PlayerController.Instance;
        input = player.GetComponent<PlayerInput>();

        StartCoroutine(CheckIfInteractable());
        
        #if UNITY_EDITOR
        Debug.Log("Interactables found: " + interactables.Count);
        #endif
    }


    IEnumerator CheckIfInteractable()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (IInteractable interactable in interactables)
            {
                // Don't bother checking what we can't resolve
                if (interactable.GetOwner() is null)
                {
                    interactables.Remove(interactable);
                    continue;
                }

                // Check if the interactable is within range
                if (interactable.IsInteractable(player.gameObject) &&
                    Vector3.Distance(interactable.GetOwner().transform.position, player.transform.position) <
                    standardInteractionDistance * interactable.GetInteractionDistanceMultiplier())
                {
                    #if UNITY_EDITOR
                    Debug.Log("Interactable in range: " + interactable.GetOwner().name);
                    #endif
                    
                    // check if interact event in new input system is active
                    if (input.actions["Interact"].IsPressed())
                    { 
                        #if UNITY_EDITOR
                        Debug.Log("Interacting with: " + interactable.GetOwner().name);
                        #endif
                        
                        interactable.Interact();
                        
                    }
                }
                
                
            }
        }
    }
    
}
