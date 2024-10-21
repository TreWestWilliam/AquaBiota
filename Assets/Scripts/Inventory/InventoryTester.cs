using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] InventoriesManager inventoriesManager;
    [SerializeField] GameObject inventoryParent;
    [SerializeField] GameObject itemParent;
    [SerializeField] Slider rowsSlider;
    [SerializeField] Slider colsSlider;
    [SerializeField] Sprite background;

    [SerializeField] InventoryItem item;

    public GameObject inventory;

    public void ApplySize()
    {
        if (inventory == null)
        {
            InventorySpace newSpace = new InventorySpace(new Vector2Int((int)rowsSlider.value, (int)colsSlider.value));
            inventoriesManager.createInventory(newSpace, background, "Test Inventory");
            inventory = inventoryParent.transform.GetChild(0).gameObject;
            inventory.SetActive(true);
        }
        else
        {
            InventorySpace newSpace = new InventorySpace(new Vector2Int((int)rowsSlider.value, (int)colsSlider.value));
            inventoriesManager.createInventory(newSpace, background, "Test Inventory");
            inventory = inventoryParent.transform.GetChild(1).gameObject;
            inventory.SetActive(true);
            Destroy(inventoryParent.transform.GetChild(0).gameObject);
        }
    }

    public void AddItem()
    {
        InventoryItem newItem = Instantiate(item, itemParent.transform);
        newItem.flipHorizontal();
        newItem.rotateWiddershins();
        if (!inventory.GetComponent<Inventory>().add(newItem, new Vector2Int(1, 1)))
        {
            Destroy(newItem.gameObject);
        }
    }
}
