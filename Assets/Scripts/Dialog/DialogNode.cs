using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogNode
{
    public string dialog;
    //[SerializeField] public DialogNode nextNodes;
    [SerializeField] public List<DialogNode> nextNodes;

    public DialogNode GetNextNode(int index)
    {
        if (nextNodes.Count < index+1)
            return null;
        
        return nextNodes[index];
    }
    
    
    public List<DialogNode> GetNextNodes()
    {
        return nextNodes;
    }
}
