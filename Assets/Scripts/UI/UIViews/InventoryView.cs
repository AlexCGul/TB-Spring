using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class InventoryView : VisualElement
{
    public static InventoryView iv;
    const string styleSheet = "GUIElement";
    private const string BGElement = "GUIElement";

    //#if UNITY_EDITOR
    Texture2D testSprite;
    [UxmlAttribute, Tooltip("NOTE: Values added here will *not* carry to the build")]
    Texture2D TestSprite
    {
        set
        {
            testSprite = value;
            style.backgroundImage = new StyleBackground(testSprite);
        } 
        get => testSprite;
    }
    
    //#endif
    public InventoryView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>(styleSheet));
        AddToClassList(BGElement);

        iv = this;
    }


    public void Pickup(Pickup pickedUp)
    {
        style.backgroundImage = new StyleBackground(pickedUp.sprite);
    }
}
