using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/New Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] DialogNode startNode;
    public string speakerName;
    
    [SerializeField] UnityEvent onDialogStart;
    [SerializeField] UnityEvent onDialogEnd;

    public DialogNode GetStartNode()
    {
        onDialogStart.Invoke();
        return startNode;
    }


    public void DialogFinished()
    {
        onDialogEnd.Invoke();
    }
}
