using System;

[Serializable]
public class DialogNode
{
    public string dialog;
    DialogNode[] nextNodes;

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
