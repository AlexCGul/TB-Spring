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

    [SerializeField] private float talkDistance = 5f;
    
    [SerializeField] UnityEvent onSatisfied;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oc = PlayerController.Instance.GetComponent<ObjectiveContainer>();
    }


    void StartDialog()
    { 
        // Don't complete the quest during the dialog to give the quest
        if (dialogIndex > 0)
            oc.AttemptCompleteDeliveryTask();
        
        if (!questComplete && dialogIndex == 2)
            dialogIndex = 1;
        
        if (dialogIndex == 1 && oc.QuestFinished())
        {
            dialogIndex = 2;
            questComplete = true;
            onSatisfied?.Invoke();
        }

        if (questDialog.Length < 1)
        {
            dialogIndex = 0;
            Debug.LogWarning("Only one dialog on " + gameObject.name);
        }

        GoToTalkPoint();
        DialogUI.instance.StartInteraction(gameObject, questDialog[dialogIndex]);
        dialogIndex += dialogIndex < 2 ? 1 : 0;
    }


    void GoToTalkPoint()
    {
        Vector3 dPos = transform.position;
        Vector3 pPos = PlayerController.Instance.transform.position;
        Vector3 dir = (pPos - dPos).normalized;
        Vector3 xDest = new Vector3(dPos.x + (dir.x * talkDistance), dPos.y, dPos.z);
        Vector3 yDest = new Vector3(dPos.x, dPos.y, dPos.z + (dir.z * talkDistance));

        if (Mathf.Abs(dir.x) > 0.25f && Vector3.Distance(pPos, xDest) < Vector3.Distance(pPos, yDest))
        {
            PlayerController.Instance.GoToDest(xDest);
            return;
        }

        PlayerController.Instance.GoToDest(yDest);
        
        
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
