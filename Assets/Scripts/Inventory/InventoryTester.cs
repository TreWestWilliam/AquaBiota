using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] InventoriesManager inventoriesManager;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] Slider rowsSlider;
    [SerializeField] Slider colsSlider;
    [SerializeField] Sprite background;


    public void ApplySize()
    {
        InventorySpace newSpace = new InventorySpace(new Vector2Int((int)rowsSlider.value, (int)colsSlider.value));
        inventoriesManager.createInventory(newSpace, background, "Test Inventory");
        //inventoryManager.UpdateInventory((int)rowsSlider.value, (int)colsSlider.value);

    }
}
