using Unity.VectorGraphics;
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private InputUIPrompts inputKey;
    [SerializeField] private SVGImage buttonImage;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text interactDescription;

    private void Update()
    {
        Quaternion lookRotation = Camera.main.transform.rotation;
        transform.rotation = lookRotation;
    }

    public void setInteractDescription(string description)
    {
        interactDescription.text = description;
    }

    public void setButtonAppearance(controllerType controller)
    {
        button interact = inputKey.getController(controller).interact;
        buttonImage.sprite = inputKey.getButtonSprite(interact);
        buttonImage.color = interact.spriteColor;
        if(interact.buttonLabel != null)
        {
            buttonText.text = interact.buttonLabel;
            buttonText.color = interact.labelColor;
            buttonText.gameObject.SetActive(true);
        }
        else
        {
            buttonText.gameObject.SetActive(false);
        }
    }
}
