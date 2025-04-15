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
            if (tasks[i].isCompleted) continue;
            tasks[i].isCompleted = true;
            break;
        }
    }


    public void UncompleteLastTask()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].isCompleted) continue;
            tasks[i-1].isCompleted = true;
            break;
        }
    }


    public Task GetNextTask()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].isCompleted)
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
            if (task.isCompleted) continue;
            if (task.taskName.Contains(itemName))
            {
                #if UNITY_EDITOR
                Debug.Log($"Objective {objectiveName} completed by {itemName}");
                #endif
                
                task.isCompleted = true;
                return true;
            }
        }

        return false;
    }
    
    
    public void UncompleteByName(string itemName)
    {
        foreach (Task task in tasks)
        {
            if (!task.isCompleted) continue;
            if (task.taskName == itemName)
            {
                task.isCompleted = false;
                break;
            }
        }
    }
}
