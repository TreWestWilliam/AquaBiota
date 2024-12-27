using UnityEngine;
using UnityEngine.UIElements;

public class InventoriesManager : MonoBehaviour
{
    public static InventoriesManager instance;

    public RectTransform inventoryParent;
    public RectTransform looseItemParent;

    [SerializeField] private Inventory inventoryPrefab;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private InventoryNavigator inventoryNavigator;
    private GameObject playerObj;

    [SerializeField] Sprite background;

    [SerializeField] private Canvas inventoryCanvas;

    [SerializeField] private float GridSize;

    [SerializeField] private bool demo = true;

    public Inventory CurrentInventory;
    
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

        playerObj = GameObject.FindAnyObjectByType<Player>().gameObject;
        if (playerObj == null ) { Debug.LogWarning("InventoriesManager couldnt find player object for stuff"); }

        if ( inventoryCanvas==null)
        {
            inventoryCanvas = GetComponentInChildren<Canvas>();
            if ( inventoryCanvas == null ) 
            {
                Debug.LogWarning("Failed to find Inventory Canvas.");
            }
        }

        if (inventoryCanvas != null  && demo ) 
        {// see if we can run canvas stuff
            inventoryCanvas.gameObject.SetActive(true);
            CreateDemoInventory();
            inventoryCanvas.gameObject.SetActive(false);
        }

        if (inventoryNavigator == null) 
        {
            GetComponentInChildren<InventoryNavigator>();
        }

    }


    public void CreateDemoInventory() 
    {
        InventorySpace newSpace = new InventorySpace(new Vector2Int(5, 5));
        Inventory inventory = createInventory(newSpace, background, "Test Inventory");
        inventory.gameObject.SetActive(true);
        CurrentInventory= inventory;
        FindAnyObjectByType<Player>().SetInventory(inventory);
        //inventory.initi
        //inventory.initialize();
        inventoryNavigator.UpdateInventory(inventory);
        inventoryNavigator.OpenInventory();
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
    /// <summary>
    /// Spawns an item near the player, used for when the player closes the inventory with an item selected or otherwise disposes of an item
    /// </summary>
    /// <param name="item"></param>
    public void CreateObjectAtPlayer(ItemData item) 
    {
        if (playerObj == null) { Debug.LogWarning("InventoriesManager couldnt find player object for stuff");  return; }
        GameObject.Instantiate(item.itemObject, playerObj.transform.position + new Vector3(Random.Range(-5,5), Random.Range(-5,5), Random.Range(-5,5)), Quaternion.identity);
    }

    public void EnableMenu() { inventoryCanvas.gameObject.SetActive(true); }
    public void DisableMenu() { inventoryCanvas.gameObject.SetActive(false);}
}
