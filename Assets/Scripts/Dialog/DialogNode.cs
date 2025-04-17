using System;
using UnityEngine;

[Serializable]
public class DialogNode
{
    public string dialog;
    [SerializeField] DialogNode[] nextNodes;

    public DialogNode GetNextNode(int index)
    {
        if (nextNodes.Length < index+1)
            return null;
        return nextNodes[index];
    }
    
    
    public DialogNode[] GetNextNodes()
    {
        return nextNodes;
    }
}
