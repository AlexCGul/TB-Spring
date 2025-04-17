using System;
using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objectives/New Objective")]
public class Objective : ScriptableObject
{
    public string objectiveName;
    public Task[] tasks;
    
    public void CompleteNextTask()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].IsCompleted) continue;
            tasks[i].IsCompleted = true;
            break;
        }
    }


    public void UncompleteLastTask()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].IsCompleted) continue;
            tasks[i-1].IsCompleted = true;
            break;
        }
    }


    public Task GetNextTask()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].IsCompleted)
            {
                return tasks[i];
            }
        }

        return null;
    }


    public bool CompleteByName(string itemName)
    {
        foreach (Task task in tasks)
        {
            if (task.IsCompleted) continue;
            if (task.taskName.Contains(itemName))
            {
                #if UNITY_EDITOR
                Debug.Log($"Objective {objectiveName} completed by {itemName}");
                #endif
                
                task.IsCompleted = true;
                return true;
            }
        }

        return false;
    }
    
    
    public void UncompleteByName(string itemName)
    {
        foreach (Task task in tasks)
        {
            if (!task.IsCompleted) continue;
            if (task.taskName == itemName)
            {
                task.IsCompleted = false;
                break;
            }
        }
    }
}
