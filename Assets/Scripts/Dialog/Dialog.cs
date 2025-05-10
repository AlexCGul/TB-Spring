using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/New Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] DialogNode[] nodes;
    private DialogNode startNode;
    
    public string speakerName;
    
    [SerializeField] UnityEvent onDialogStart;
    [SerializeField] UnityEvent onDialogEnd;
    
    public DialogNode GetStartNode()
    {
        // initialize the nodes
        startNode = nodes[0];
        DialogNode currentNode = startNode;
        // construct the tree out of currentnode from the nodes data

        for (int j = 1; j < nodes.Length; j++)
        {
            if (nodes[j] != null)
            {
                currentNode.nextNodes.Add(nodes[j]);
                currentNode = nodes[j];
            }
           
        }
        
        
        onDialogStart.Invoke();
        return nodes[0];
    }


    public void DialogFinished()
    {
        onDialogEnd.Invoke();
    }
}
