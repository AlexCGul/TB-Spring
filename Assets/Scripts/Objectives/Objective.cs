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
}
