using System.Collections;
using UnityEngine;

public class Dialogable : MonoBehaviour, IInteractable
{
    private ObjectiveContainer oc;
    [SerializeField] private Dialog[] questDialog;
    [SerializeField] int dialogIndex = 0;
    [SerializeField] private bool questComplete = false;
    DialogNode currentNode;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oc = PlayerController.Instance.GetComponent<ObjectiveContainer>();
    }


    IEnumerator StartDialog()
    {
        switch (dialogIndex)
        {
            case 1:
                if (oc.AttemptCompleteDeliveryTask())
                {
                    dialogIndex = 1;
                    questComplete = true;
                }
                break;
            
            case 2:
                if (!questComplete)
                    dialogIndex = 1;
                break;
        }
        
        
        currentNode = questDialog[dialogIndex].GetStartNode();
        while (currentNode is not null)
        { 
            #if UNITY_EDITOR
            // Display dialog
            Debug.Log(currentNode.dialog);
            #endif

            yield return new WaitForSeconds(1f);
            
            // Move to the next node
            currentNode = currentNode?.GetNextNode(0);
        }
        
        questDialog[dialogIndex].DialogFinished();
        dialogIndex += dialogIndex < 2 ? 1 : 0;
    }

    public void Interact()
    {
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
