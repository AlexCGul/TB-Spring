using System.Collections;
using UnityEngine;

public class Dialogable : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialog dialog;
    DialogNode currentNode;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    IEnumerator StartDialog()
    {
        while (currentNode is not null)
        { 
            #if UNITY_EDITOR
            // Display dialog
            Debug.Log(currentNode.dialog);
            #endif

            yield return new WaitForSeconds(1f);
            
            // Move to the next node
            currentNode = currentNode.GetNextNode(0);
        }
        
        dialog.DialogFinished();
    }

    public void Interact()
    {
        currentNode = dialog.GetStartNode();
        StartCoroutine(StartDialog());
    }

    public bool IsInteractable(GameObject interactingCharacter)
    {
        return true;
    }

    public string GetInteractionType()
    {
        return "Talk";
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
