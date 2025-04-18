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
    private Dialog currentDialog;
    
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
        currentDialog = dialog;
        
        // setup textbox
        title.text = dialog.name;
        currentNode = dialog.GetStartNode();
        dialogRoot.style.display = DisplayStyle.Flex;
        Camera cam = Camera.main;
        
        // Set box position
        VisualElement dialogBox = dialogRoot.Q("DialogBox");
        Vector3 screenPos = cam.WorldToScreenPoint(actor.transform.position);
        dialogBox.style.left = new StyleLength(new Length(screenPos.x, LengthUnit.Pixel));
        dialogBox.style.top = new StyleLength(new Length(cam.pixelHeight - screenPos.y, LengthUnit.Pixel));
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
        if (currentNode == null)
        {
            EndDialog();
            return;
        }
        
        StartCoroutine(WriteText(currentNode.dialog, body));
        currentNode = currentNode.GetNextNode(0);
    }


    void EndDialog()
    {
        dialogRoot.style.display = DisplayStyle.None;
        
        // Cleanup refs
        currentDialog.DialogFinished();
        currentNode = null;
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
