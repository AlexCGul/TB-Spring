using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InteractionManager : MonoBehaviour
{
    private Dictionary<IInteractable, bool> interactables;
    private PlayerController player;
    PlayerInput input;
    [SerializeField] UIDocument document;
    [SerializeField] private float standardInteractionDistance;

    private VisualElement interactionWidgetsRoot;
    List<InteractWidget> widgets = new List<InteractWidget>();
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get all the interactables
        interactables = new Dictionary<IInteractable, bool>(FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>()
            .ToDictionary(interactable => interactable, interactable => false));
        player = PlayerController.Instance;
        input = player.GetComponent<PlayerInput>();
        
        interactionWidgetsRoot = document.GetComponent<UIDocument>().rootVisualElement.Q("InteractWidgets");

        StartCoroutine(CheckIfInteractable());
        StartCoroutine(UpdateWidgets());
        
        #if UNITY_EDITOR
        Debug.Log("Interactables found: " + interactables.Count);
        #endif
    }


    IEnumerator CheckIfInteractable()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(0.5f);
            for (int x = 0; x < interactables.Count; x++)
            {
                IInteractable interactable = interactables.ElementAt(x).Key;
                
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

                    // check if interactable already has a widget
                    if (!interactables.ElementAt(x).Value)
                    {
                        #if UNITY_EDITOR
                        Debug.Log("Adding Widget For: " + interactable.GetOwner().name);
                        #endif
                        
                        InteractWidget widget = new InteractWidget();
                        widgets.Add(widget);
                        interactionWidgetsRoot.Add(widget);
                        widget.target = interactable.GetOwner().transform;
                        interactables[interactable] = true;
                    }

                    // check if interact event in new input system is active
                    if (input.actions["Interact"].IsPressed())
                    { 
                        #if UNITY_EDITOR
                        Debug.Log("Interacting with: " + interactable.GetOwner().name);
                        #endif
                        
                        interactable.Interact();
                        
                    }

                    continue;
                }
                
                if (interactables.ElementAt(x).Value)
                {
                    #if UNITY_EDITOR
                    Debug.Log("Removing Widget For: " + interactable.GetOwner().name);
                    #endif
                    
                    // remove the widget
                    for (int i = 0; i < widgets.Count; i++)
                    {
                        if (widgets[i].target == interactable.GetOwner().transform)
                        {
                            interactionWidgetsRoot.Remove(widgets[i]);
                            interactables[interactable] = false;
                            widgets.RemoveAt(i);
                            break;
                        }
                    }
                }
                
            }
        }
    }


    IEnumerator UpdateWidgets()
    {
        while (gameObject)
        {
            // update all widgets
            for (int i = 0; i < widgets.Count; i++)
            {
                widgets[i].Update();
            }

            yield return new WaitForFixedUpdate();
        }
    }
    
}
