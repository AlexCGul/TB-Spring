using UnityEngine;

public interface IInteractable
{
    public void Interact(Transform interactingCharacter);
    public string GetInteractionType();
}
