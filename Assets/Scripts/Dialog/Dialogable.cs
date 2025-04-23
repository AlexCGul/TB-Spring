using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Dialogable : MonoBehaviour, IInteractable
{
    private ObjectiveContainer oc;
    [SerializeField] private Dialog[] questDialog;
    [SerializeField] int dialogIndex = 0;
    [SerializeField] private bool questComplete = false;
    DialogNode currentNode;
    
    [SerializeField] UnityEvent onSatisfied;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oc = PlayerController.Instance.GetComponent<ObjectiveContainer>();
    }


    void StartDialog()
    {
        switch (dialogIndex)
        {
            case 1:
                if (oc.AttemptCompleteDeliveryTask())
                {
                    dialogIndex = 1;
                    questComplete = true;
                    onSatisfied?.Invoke();
                }

                break;

            case 2:
                if (!questComplete)
                    dialogIndex = 1;
                break;
        }

        if (questDialog.Length < 1)
        {
            dialogIndex = 0;
            Debug.LogWarning("Only one dialog on " + gameObject.name);
        }

        DialogUI.instance.StartInteraction(gameObject, questDialog[dialogIndex]);
        dialogIndex += dialogIndex < 2 ? 1 : 0;
    }

    public void Interact()
    {
        StartDialog();
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
