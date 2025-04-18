using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class InteractWidget : VisualElement
{
    const string styleSheet = "GUIElement";
    private const string BGElement = "GUIElement";
    private Camera main;
    
    public Transform target;
    
    public InteractWidget()
    {
        styleSheets.Add(Resources.Load<StyleSheet>(styleSheet));
        AddToClassList(BGElement);
        
        style.minWidth = new StyleLength(new Length(50, LengthUnit.Pixel));
        style.minHeight = new StyleLength(new Length(50, LengthUnit.Pixel));
        style.position = Position.Absolute;
        
        main = Camera.main;
        Update();
    }


    public void Update()
    {
        if (target)
        {
            // get screen height
            Vector3 screenPos = main.WorldToScreenPoint(target.position);
            style.left = new StyleLength(new Length(screenPos.x, LengthUnit.Pixel));
            style.top = new StyleLength(new Length(main.pixelHeight - screenPos.y, LengthUnit.Pixel));
        }
    }
}
