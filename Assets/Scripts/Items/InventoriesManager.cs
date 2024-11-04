using UnityEngine;

public class InventoriesManager : MonoBehaviour
{
    public static InventoriesManager instance;

    public RectTransform inventoryParent;
    public RectTransform looseItemParent;

    [SerializeField] private Inventory inventoryPrefab;
    [SerializeField] private GameObject inventorySlotPrefab;

    [SerializeField] private float GridSize;
    public float gridSize
    {
        get
        {
            return GridSize;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Inventory createInventory(InventorySpace shape, Sprite background, string inventoryName)
    {
        Inventory inventory = Instantiate(inventoryPrefab, inventoryParent);

        inventory.space = new InventorySpace(shape);
        inventory.contents = new InventorySpace(shape);
        inventory.setBackground(background);
        inventory.name = inventoryName;

        return inventory;
    }
}
