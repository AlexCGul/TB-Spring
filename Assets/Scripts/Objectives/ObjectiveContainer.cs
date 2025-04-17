using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public delegate void ObjectiveAdded(Objective objective);
public class ObjectiveContainer : MonoBehaviour
{
    [SerializeField] private Objective objective;
    public Objective GetActiveObjective => objective;
    
    public ObjectiveAdded OnObjectiveAdded;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    public void AttemptCompletePickupTask(Pickup pickup)
    {
        objective.CompleteByName("Pickup " + pickup.name);
    }
    
    
    public void AddObjective(Objective newObjective)
    {
        this.objective = newObjective;
        
        foreach (Task task in newObjective.tasks)
        {
            task.IsCompleted = false;
        }
        
        // why doesn't the event worK??????
        //OnObjectiveAdded?.Invoke(newObjective);
        ObjectiveView.ov.AddNewObjective(newObjective);
        #if UNITY_EDITOR
        Debug.Log($"Objective {objective.objectiveName} added to the container.");
        #endif
    }
    
}
