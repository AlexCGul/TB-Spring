using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectiveContainer : MonoBehaviour
{
    [SerializeField] private List<Objective> objectives;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectives.Clear();
    }


    public void AttemptCompletePickupTask(Pickup pickup)
    {
        foreach (Objective objective in objectives)
        {
            if (objective.CompleteByName("Pickup " + pickup.name))
            {
                break;
            }
        }
    }
    
    
    public void AddObjective(Objective objective)
    {
        objectives.Add(objective);
        
        foreach (Task task in objective.tasks)
        {
            task.isCompleted = false;
        }
        
        #if UNITY_EDITOR
        Debug.Log($"Objective {objective.objectiveName} added to the container.");
        #endif
    }
    
}
