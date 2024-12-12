using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InteractUI interactUI;

    [SerializeField] private Transform interactionCenter;
    [SerializeField] private int currentInteractable;
    [SerializeField] private List<InteractableObject> interactables;

    [SerializeField] private Sprayer sprayer;

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float characterWeight;

    [SerializeField] private float inventoryWeightMultiplier;

    private void Awake()
    {
        if(interactables == null)
        {
            interactables = new List<InteractableObject>();
        }
    }

    private void Start()
    {
        updateWeight();
    }

    private void FixedUpdate()
    {
        OceanManager.instance.recenterWaves(transform.position);
        setCurrentInteractable();
    }

    public void setControllerUI(controllerType type)
    {
        interactUI.setButtonAppearance(type);
    }

    public void sprayCleaner()
    {
        if(sprayer.sprayCleaner())
        {
            updateWeight();
        }
    }

    public void updateWeight()
    {
        float totalWeight = characterWeight;
        if(sprayer!=null)
            totalWeight += sprayer.getWeight();
        if (inventory != null) 
            totalWeight += (inventory.getWeight() * inventoryWeightMultiplier);
        if (rigidBody != null)
            rigidBody.mass = totalWeight;
    }

    public void openInventory(InventoryItem item)
    {
        InventoryItem[] items = { item };
        openInventory(items);
    }

    public void openInventory(InventoryItem[] items)
    {
        foreach(InventoryItem item in items)
        {
            MenuManager.Instance.openMenu();
            inventory.items.Add(item);
            item.itemObject.endInteraction(this, true);
            MenuManager.Instance.closeMenu();
            updateWeight();
        }
    }

    public void swapInteractFocus()
    {
        if(interactables.Count > 1)
        {
            currentInteractable = (currentInteractable + 1) % interactables.Count;
        }
        else
        {
            currentInteractable = 0;
        }
    }

    public void Interact()
    {
        if(interactables.Count > 0)
        {
            interactables[currentInteractable].beginInteraction(this);
            removeInRangeInteractable(interactables[currentInteractable]);
        }
    }

    public void closeInventory()
    {

    }

    private void setCurrentInteractable()
    {
        if(interactables.Count > 1)
        {
            InteractableObject first = interactables[0];
            InteractableObject selected = interactables[currentInteractable];

            interactables = interactables.OrderBy((d) => (d.getPosition() - interactionCenter.position).sqrMagnitude).ToList();

            if(first != interactables[0])
            {
                currentInteractable = 0;
                setSelectedInteractable(interactables[0]);
            }
            else if(selected != first)
            {
                currentInteractable = interactables.IndexOf(selected);
            }
            positionInteractUI(interactables[currentInteractable]);
        }
    }

    private void positionInteractUI(InteractableObject interactable)
    {
        interactUI.transform.position = interactable.getPosition();
        interactUI.transform.SetParent(interactable.getTransform());
    }

    private void setSelectedInteractable(InteractableObject interactable)
    {
        positionInteractUI(interactable);
        interactUI.setInteractDescription(interactable.getInteractText());
    }

    private void addInRangeInteractable(InteractableObject interactable)
    {
        interactables.Add(interactable);
        if(interactables.Count == 1)
        {
            currentInteractable = 0;
            setSelectedInteractable(interactable);
            interactUI.gameObject.SetActive(true);
        }
    }

    private void removeInRangeInteractable(InteractableObject interactable)
    {
        if(interactables[currentInteractable] == interactable)
        {
            currentInteractable = 0;
            setSelectedInteractable(interactable);
        }
        interactables.Remove(interactable);
        if(interactables.Count == 0)
        {
            interactUI.gameObject.SetActive(false);
            interactUI.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if(interactable != null && !interactables.Contains(interactable))
        {
            addInRangeInteractable(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if(interactable != null && interactables.Contains(interactable))
        {
            removeInRangeInteractable(interactable);
        }
    }
}
