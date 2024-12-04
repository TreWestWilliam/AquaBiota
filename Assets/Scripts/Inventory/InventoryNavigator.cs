using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryNavigator : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject selectionBox;

    public Inventory inventory;
    public Vector2Int currentPosition;

    public InventoryItem heldItem;
    private bool holdingItem = false;

    private Vector2Int size;
    private bool inventoryOpen;
    private float inputTimer = 0f;

    private Vector2 lastNavigateInput;

    void Update()
    {
        if (!inventoryOpen) { return; }
        bool select = playerInput.actions["Submit"].triggered;
        if(select) SelectSquare();

        bool isRotating = playerInput.actions["Rotate"].triggered;
        if (isRotating)
        {
            float rotation = playerInput.actions["Rotate"].ReadValue<float>();
            RotateSelection(rotation);
        }

        bool isFlipping = playerInput.actions["Flip"].triggered;
        if (isFlipping)
        {
            float flip = playerInput.actions["Flip"].ReadValue<float>();
            FlipSelection(flip);
        }

        Vector2 navigateInput = playerInput.actions["Navigate"].ReadValue<Vector2>();
        if (navigateInput.sqrMagnitude == 0f )
        {
            inputTimer = 0f;
            lastNavigateInput = navigateInput;
            return;
        }

        if (lastNavigateInput == navigateInput)
        {
            if (inputTimer > 0.35f)
            {
                moveCursor(navigateInput);

                inputTimer -= 0.08f;
            }
            inputTimer += Time.deltaTime;
        }
        else
        {
            // Check to make sure button wasn't held
            if ((navigateInput.x > 0 && lastNavigateInput.x > 0) || (navigateInput.x < 0 && lastNavigateInput.x < 0)) moveCursor(new Vector2(0, navigateInput.y));
            else if ((navigateInput.y > 0 && lastNavigateInput.y > 0) || (navigateInput.y < 0 && lastNavigateInput.y < 0)) moveCursor(new Vector2(navigateInput.x, 0));
            else moveCursor(navigateInput);

            inputTimer = 0f;
            lastNavigateInput = navigateInput;
        }
    }

    public void OpenInventory()
    {
        inventoryOpen = true;
        currentPosition = new Vector2Int(0, 0);
        selectionBox.SetActive(true);
        UpdateSelectionBox();
    }
    public void CloseInventory()
    {
        if (holdingItem) 
        {
            // Spawn item in worlspace if we have an item held on close
            InventoriesManager.instance.CreateObjectAtPlayer(heldItem.itemData);
        }
        inventoryOpen = false;
        selectionBox.SetActive(false);
    }

    public void UpdateInventory(Inventory newInventory)
    {
        inventory = newInventory;
        size = inventory.space.size;
    }

    public void HoldItem(InventoryItem item)
    {
        heldItem = item;

        Color imageColor = heldItem.image.color;
        imageColor.a = 0.5f;
        heldItem.image.color = imageColor;

        selectionBox.SetActive(false);
        holdingItem = true;
        UpdateSelectionBox();
    }

    public void SelectSquare()
    {
        if (holdingItem)
        {
            if (!inventory.GetComponent<Inventory>().add(heldItem, currentPosition)) return;

            Color imageColor = heldItem.image.color;
            imageColor.a = 1f;
            heldItem.image.color = imageColor;

            selectionBox.SetActive(true);
            holdingItem = false;
            UpdateSelectionBox();
        }
        else
        {
            heldItem = inventory.GetItemByCoordinate(currentPosition);
            if(heldItem == null) return;
            inventory.remove(heldItem);

            Color imageColor = heldItem.image.color;
            imageColor.a = 0.5f;
            heldItem.image.color = imageColor;

            selectionBox.SetActive(false);
            holdingItem = true;
            UpdateSelectionBox();
        }
    }

    public void RotateSelection(float dir)
    {
        if (!holdingItem) return;
        
        if (dir > 0) heldItem.rotateClockwise();
        else if (dir < 0) heldItem.rotateWiddershins();
        UpdateSelectionBox();
    }

    public void FlipSelection(float dir)
    {
        if (!holdingItem) return;

        if (dir > 0) heldItem.flipHorizontal();
        else if (dir < 0) heldItem.flipVertical();
        UpdateSelectionBox();
    }

    private void moveCursor(Vector2 input)
    {
        Vector2Int newPostion = currentPosition;
        if (input.x > 0.5f && !(newPostion.x + 1 >= size.x)) newPostion.x += 1;
        else if (input.x < -0.5f && !(newPostion.x - 1 < 0)) newPostion.x -= 1;

        if (input.y < -0.5f && !(newPostion.y + 1 >= size.y)) newPostion.y += 1;
        else if (input.y > 0.5f && !(newPostion.y - 1 < 0)) newPostion.y -= 1;

        if(newPostion != currentPosition)
        {
            currentPosition = newPostion;
            UpdateSelectionBox();
        }
    }
    private void UpdateSelectionBox()
    {
        if (holdingItem)
        {
            // Handles displaying item
            Vector2 itemSize;
            if (heldItem.rotation.onSide())
            {
                itemSize = new Vector2(heldItem.itemData.inventoryShape.size.y, heldItem.itemData.inventoryShape.size.x) / 2f;
            }
            else
            {
                itemSize = new Vector2(heldItem.itemData.inventoryShape.size.x, heldItem.itemData.inventoryShape.size.y) / 2f;

            }
            heldItem.transform.localPosition = new Vector3((currentPosition.x - (size.x / 2f) + itemSize.x) * InventoriesManager.instance.gridSize,
                -(currentPosition.y - (size.y / 2f) + itemSize.y + 0.5f) * InventoriesManager.instance.gridSize, 0f);
        }
        else
        {
            selectionBox.transform.localPosition = new Vector3((currentPosition.x - (size.x / 2f) + 0.5f) * InventoriesManager.instance.gridSize,
                -(currentPosition.y - (size.y / 2f) + 0.5f) * InventoriesManager.instance.gridSize, 0f);
        }
    }
}
