using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/New Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] DialogNode startNode;
    public string speakerName;

    public DialogNode GetStartNode()
    {
        return startNode;
    }
}
