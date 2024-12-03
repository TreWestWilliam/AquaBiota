using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // defines the boundaries of the inventory
    public InventorySpace space;
    // combination of space boundaries and items placed
    public InventorySpace contents;

    public List<InventoryItem> items;

    private InventoryItem[,] itemByCoordinate;

    [SerializeField] private Image background;
    [SerializeField] private TMP_Text nameText;

    [SerializeField] private RectTransform gridParent;
    [SerializeField] private RectTransform itemParent;

    public GameObject inventorySlotPrefab;
    public GameObject[,] inventorySlots;
    [SerializeField] private GameObject tossSlot;

    private void Awake()
    {
        if(items == null)
        {
            items = new List<InventoryItem>();
        }
        initialize();
    }

    private void initialize()
    {
        itemByCoordinate = new InventoryItem[space.size.x, space.size.y];
        foreach(InventoryItem item in items)
        {
            addItemToContents(item);
        }

        gridParent.GetComponent<GridLayoutGroup>().constraintCount = space.size.x;
        inventorySlots = new GameObject[space.size.x, space.size.y];
        int width = Screen.width;
        for (int i = 0; i < space.size.y; i++)
        {
            for (int j = 0; j < space.size.x; j++)
            {
                inventorySlots[j, i] = Instantiate(inventorySlotPrefab);
                inventorySlots[j, i].transform.SetParent(gridParent, true);
                inventorySlots[j, i].transform.localScale = new Vector3(1, 1, 1); // Needed to fix scaling
            }
        }
        tossSlot.transform.localPosition = new Vector2((-50 * space.size.x) - 50, (50 * space.size.y) - 125);

        nameText.text = this.name;
    }

    public void setBackground(Sprite backgroundSprite)
    {
        background.sprite = backgroundSprite;
    }

    public bool add(InventoryItem item, Vector2Int position)
    {
        item.position = position;

        if (!contents.checkFitAndOverlap(item.itemData.inventoryShape, position, item.rotation))
        {
            Debug.Log("Item cannot fit");
            return false;
        }

        items.Add(item);
        addItemToContents(item);
        item.transform.SetParent(itemParent);

        // Handles displaying item
        Vector2 itemSize;
        if (item.rotation.onSide())
        {
            itemSize = new Vector2(item.itemData.inventoryShape.size.y, item.itemData.inventoryShape.size.x) / 2f;
        }
        else
        {
            itemSize = new Vector2(item.itemData.inventoryShape.size.x, item.itemData.inventoryShape.size.y) / 2f;
            
        }
        item.transform.localPosition = new Vector3((position.x - (space.size.x / 2f) + itemSize.x) * InventoriesManager.instance.gridSize,
                -(position.y - (space.size.y / 2f) + itemSize.y) * InventoriesManager.instance.gridSize, 0f);
        return true;
    }

    public void remove(InventoryItem item)
    {
        removeItemFromContents(item);
        items.Remove(item);
        item.position = Vector2Int.zero;
        item.transform.SetParent(InventoriesManager.instance.looseItemParent);
    }

    private void addItemToContents(InventoryItem item)
    {
        Vector2Int[] coordinates = contents.addOtherCoord(item.itemData.inventoryShape, item.position, item.rotation);
        foreach(Vector2Int coordinate in coordinates)
        {
            itemByCoordinate[coordinate.x, coordinate.y] = item;
        }
    }

    private void removeItemFromContents(InventoryItem item)
    {
        if(items.Contains(item))
        {
            Vector2Int[] coordinates = contents.subtractOtherCoord(item.itemData.inventoryShape, item.position, item.rotation);
            foreach(Vector2Int coordinate in coordinates)
            {
                itemByCoordinate[coordinate.x, coordinate.y] = null;
            }
        }
    }

    public InventoryItem GetItemByCoordinate(Vector2Int position)
    {
        return itemByCoordinate[position.x, position.y];
    }

    public float getWeight()
    {
        float weight = 0f;
        foreach(InventoryItem item in items)
        {
            weight += item.weight;
        }
        return weight;
    }
}
