using UnityEditor.Search;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    private void FixedUpdate()
    {
        OceanManager.instance.recenterWaves(transform.position);
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
            inventory.items.Add(item);
            item.itemObject.gameObject.SetActive(false);
            item.itemObject.endInteraction(this);
        }
    }

    public void closeInventory()
    {

    }
}
