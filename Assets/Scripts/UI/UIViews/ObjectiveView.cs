using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class ObjectiveView : VisualElement
{
    public static ObjectiveView ov;
    // Used resources
    const string styleSheet = "GUIElement";
    private const string BGElement = "GUIElement";
    private const string titleText = "GUI_TextBase";
    private const string SubText = "GUI_SubText";
    const string objectiveText = "ObjectiveText";
    
    // Use this for testing purposes only
    //#if UNITY_EDITOR
    private string[] tasks;
    
    [UxmlAttribute, Tooltip("NOTE: Values added here will *not* carry to the build")]
    public string[] Tasks
    {
        set
        {
            tasks = value;
            RemoveAllTasks();

            foreach (string task in value)
            {
                AddTask(new Task(task));
            }
        } 
        get => tasks;
    }
//#endif
    
    // Parameters
    [UxmlAttribute] public string objectiveName = "Grab Coffee";
    
    // Tracked elements
    VisualElement taskContainer;
    private Label title;
    
    
    
    
    public ObjectiveView()
    {
        // Setup window
        styleSheets.Add(Resources.Load<StyleSheet>(styleSheet));
        AddToClassList(BGElement);
        
        // Construct UI elements
        title = new Label(objectiveName);
        taskContainer = new VisualElement();
        
        // setup styles
        title.AddToClassList(titleText);
        taskContainer.style.marginBottom = new StyleLength(new Length(10, LengthUnit.Percent));
        
        // Add everything to hierarchy
        Add(title);
        Add(taskContainer);
        ov = this;
        
        //PlayerController.OnInitialize += SetupEvents;
    }
    
    
    void SetupEvents()
    {
        
        // If playercontroller is active hook up to events
        PlayerController pc = PlayerController.Instance;
        if (pc)
        {
            #if UNITY_EDITOR
            Debug.Log($"ObjectiveView: PlayerController found, hooking up to events");
            #endif
            
            ObjectiveContainer oc = pc.GetComponent<ObjectiveContainer>();
            if (oc != null)
            {
                oc.OnObjectiveAdded += AddNewObjective;

            }
            else
            {
                Debug.Log("No Objective Container");
            }
        }
        else
        {
            Debug.LogWarning($"ObjectiveView: PlayerController not found, unable to hook up to events");
        }
    }

    public void AddNewObjective(Objective objective)
    {
        RemoveAllTasks();
        title.text = objective.objectiveName;
        foreach (Task task in objective.tasks)
        {
            AddTask(task);
        }
    }
    
    
    
    
    public void AddTask(Task task)
    {
        Label taskLabel = new Label(task.taskName);
        taskLabel.AddToClassList(SubText);
        taskLabel.AddToClassList(objectiveText);
        taskContainer.Add(taskLabel);
        Color currentColor = taskLabel.resolvedStyle.color;
        task.OnTaskCompleted += () =>
        {
            taskLabel.style.color = new StyleColor(Color.green);
        };
        
        task.OnTaskUncompleted += () =>
        {
            taskLabel.style.color = currentColor;
        };
    }


    Label GetTask(Task requestedTask)
    {
        for (int x = 0; x < taskContainer.childCount; x++)
        {
            Label taskLabel = taskContainer[x] as Label;
            if (taskLabel == null) continue;
            if (taskLabel.text == requestedTask.taskName)
            {
                return taskLabel;
            }
        }

        return null;
    }
    
    
    public void RemoveTask(Task task)
    {
        Label taskToRemove = GetTask(task);
        if (taskToRemove == null) return;
        
        taskContainer.Remove(taskToRemove);
    }
    
    
    public void CompleteTask (Task task)
    {
        Label taskToRemove = GetTask(task);
        if (taskToRemove == null) return;
        
        taskContainer.Remove(taskToRemove);
    }
    
    void RemoveAllTasks()
    {
        taskContainer.Clear();
    }
}
