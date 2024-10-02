using System;
using UnityEngine;
using UnityEngine.UI;

public enum controllerType
{
    MouseAndKeyboard,
    xBox,
    PlayStation,
    Switch
}

[Serializable]
public class button
{
    public int buttonSprite;
    public Color spriteColor;
    public string buttonLabel;
    public Color labelColor;
}

[Serializable]
public class controls
{
    public button interact;
    public button clean;
    public button swimUp;
    public button swimDown;
    public button pause;
    public button inventory;
}

[CreateAssetMenu(fileName = "InputUIPrompts", menuName = "Scriptable Objects/InputUIPrompts")]
public class InputUIPrompts : ScriptableObject
{
    [SerializeField] private Sprite[] buttonSprites;

    public controls mouseAndKeyboard;
    public controls xBox;
    public controls PlayStation;
    public controls Switch;

    public controls getController(controllerType type)
    {
        switch(type)
        {
            case controllerType.MouseAndKeyboard:
                return mouseAndKeyboard;
            case controllerType.PlayStation:
                return PlayStation;
            case controllerType.Switch:
                return Switch;
            default:
                return xBox;
        }
    }

    public Sprite getButtonSprite(button Button)
    {
        return buttonSprites[Button.buttonSprite];
    }
}
