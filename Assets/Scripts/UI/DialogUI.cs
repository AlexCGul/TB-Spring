using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DialogUI : MonoBehaviour
{
    public static DialogUI instance;
    [SerializeField] private float textWriteSpeed = 0.25f;
    
    [SerializeField] private GameObject talkingActor;
    private PlayerInput input;
    private VisualElement dialogRoot;
    private DialogNode currentNode;
    
    private Label body;
    private Label title;
    bool writing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;
        input = PlayerController.Instance.GetComponent<PlayerInput>();
        
        instance = this;
        dialogRoot = root.Q("Dialog");
        dialogRoot.style.display = DisplayStyle.None;
        
        title = dialogRoot.Q<Label>("Name");
        body = dialogRoot.Q<Label>("Body");
        
    }


    public void StartInteraction(GameObject actor, Dialog dialog)
    {
        if (dialogRoot == null)
        {
            Debug.LogError("Dialog UI's root is null");
            return;
        }
        
        // Setup input
        input.SwitchCurrentActionMap("UI");
        InputActionMap uiMap = input.actions.FindActionMap("UI");

        uiMap.FindAction("Click").performed += ctx =>
        {
            InputNext();
        };
        
        // Setup refs
        talkingActor = actor;
        
        // setup textbox
        title.text = dialog.name;
        currentNode = dialog.GetStartNode();
        dialogRoot.style.display = DisplayStyle.Flex;
        TriggerNextNode();

        
        dialog.DialogFinished();
    }


    void InputNext()
    {
        #if UNITY_EDITOR
        Debug.Log("InputNext");
        #endif

        if (writing)
        {
            writing = false;
            return;
        }
        
        TriggerNextNode();
    }
    

    void TriggerNextNode()
    {
        StartCoroutine(WriteText(currentNode.dialog, body));
        currentNode = currentNode.GetNextNode(0);

        if (currentNode == null)
        {
            EndDialog();
        }
    }


    void EndDialog()
    {
        dialogRoot.style.display = DisplayStyle.None;
        talkingActor = null;
        
        InputActionMap uiMap = input.actions.FindActionMap("UI");

        // return input back to normal
        uiMap.FindAction("Click").performed -= ctx =>
        {
            InputNext();
        };
        input.SwitchCurrentActionMap("Player");

    }


    IEnumerator WriteText(string dialog, Label target)
    {
        target.text = "";
        writing = true;
        
        for (int i = 0; i < dialog.Length && writing; i++)
        {
            target.text += dialog[i];
            yield return new WaitForSeconds(textWriteSpeed);
        }
        
        writing = false;
        target.text = dialog;
    }
}
