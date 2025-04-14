using UnityEngine;

public interface IInteractable
{
    // Activate the interactable thing
    public void Interact();
    
    // Check whether or not the interactable is interactable
    public bool IsInteractable(GameObject interactingCharacter);
    
    // Get the type of interaction [EX) "Use", "Talk", "Pick Up"]
    public string GetInteractionType();
    
    // Gets the gameobject of the interactable
    public GameObject GetOwner();
    
    // How much to multiply the interaction distance by. Mostly for large objects
    public float GetInteractionDistanceMultiplier();
}
