using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] InventoriesManager inventoriesManager;
    [SerializeField] InventoryNavigator inventoryNavigator;
    [SerializeField] GameObject inventoryParent;
    [SerializeField] GameObject itemParent;
    [SerializeField] Slider rowsSlider;
    [SerializeField] Slider colsSlider;
    [SerializeField] TMP_Text rowsNum;
    [SerializeField] TMP_Text colsNum;
    [SerializeField] Sprite background;

    [SerializeField] InventoryItem[] items;

    public GameObject inventory;
    private InventoryItem itemToDelete;
    public void Update()
    {
        
    }

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

        inventoryNavigator.UpdateInventory(inventory.GetComponent<Inventory>());
        inventoryNavigator.OpenInventory();
    }

    public void AddItem(int itemSelection)
    {
        InventoryItem newItem = Instantiate(items[itemSelection], itemParent.transform);
        inventoryNavigator.HoldItem(newItem);
    }
}
