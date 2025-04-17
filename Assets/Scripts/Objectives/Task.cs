using System;

[Serializable]
public class Task
{
    bool isCompleted;
    public string taskName;
    
    public event Action OnTaskCompleted;
    public event Action OnTaskUncompleted;
    
    public bool IsCompleted
    {
        get => isCompleted;
        set
        {
            if (!isCompleted && value)
            {
                OnTaskCompleted?.Invoke();
            }
            else if (isCompleted && !value)
            {
                OnTaskUncompleted?.Invoke();
            }
            
            isCompleted = value;

        }
    }
    
    public Task(string taskName)
    {
        this.taskName = taskName;
        isCompleted = false;
    }
}
