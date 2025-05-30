using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public delegate void ObjectiveAdded(Objective objective);
public class ObjectiveContainer : MonoBehaviour
{
    [SerializeField] private Objective objective;
    [SerializeField] Inventory inventory;
    public Objective GetActiveObjective => objective;
    
    public ObjectiveAdded OnObjectiveAdded;

    private void Start()
    {
        inventory = null;
    }

    public void AttemptCompletePickupTask(Pickup pickup)
    {
        objective.CompleteByName("Pickup " + pickup.name);
    }


    public bool QuestFinished()
    {
        foreach (Task task in objective.tasks)
        {
            if (!task.IsCompleted && task.taskName.Contains("Bring"))
            {
                return false;
            }
        }

        return true;
    }
    
    
    public bool AttemptCompleteDeliveryTask()
    {
        if (!inventory)
            inventory = PlayerController.Instance.GetComponent<Inventory>();

        if (!inventory || !inventory.GetHeld())
            return false;
        
        bool completed = objective.CompleteByName("Bring " + inventory.GetHeld().name);
        
        // If pickup objective is completed then delete it
        if (completed)
        {
            inventory.DeleteItem();
        }
        
        return completed;
    }
    
    
    public void AddObjective(Objective newObjective)
    {
        objective = newObjective;
        
        foreach (Task task in newObjective.tasks)
        {
            task.IsCompleted = false;
        }
        
        // Inventory is null?? Idk why so this'll fix it?
        if (!inventory)
            inventory = PlayerController.Instance.GetComponent<Inventory>();

        
        // why doesn't the event worK??????
        //OnObjectiveAdded?.Invoke(newObjective);
        ObjectiveView.ov.AddNewObjective(newObjective);
        
        if (inventory && inventory.GetHeld())
            AttemptCompletePickupTask(inventory.GetHeld());

        #if UNITY_EDITOR
        Debug.Log($"Objective {objective.objectiveName} added to the container.");
        #endif
    }
}
