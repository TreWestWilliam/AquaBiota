using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] InventoryManager inventory;
    [SerializeField] Slider rowsSlider;
    [SerializeField] Slider colsSlider;

    public void ApplySize()
    {
        inventory.UpdateInventory((int)rowsSlider.value, (int)colsSlider.value);
    }
}
