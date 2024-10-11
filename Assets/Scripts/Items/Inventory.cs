using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventorySpace space;

    public InventorySpace contents;

    public List<InventoryItem> items;

    private InventoryItem[,] itemByCoordinate;

    [SerializeField] private Image background;

    [SerializeField] private RectTransform itemParent;

    public GameObject inventorySlotPrefab;
    public GameObject[,] inventorySlots;

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

        Debug.Log(this.transform.name);
        Transform inventoryBody = this.transform.Find("Inventory Body");
        Debug.Log(inventoryBody.name);
        inventoryBody.GetComponent<GridLayoutGroup>().constraintCount = space.size.x;
        inventorySlots = new GameObject[space.size.x, space.size.y];
        for (int i = 0; i < space.size.y; i++)
        {
            for (int j = 0; j < space.size.x; j++)
            {
                inventorySlots[j, i] = Instantiate(inventorySlotPrefab);
                inventorySlots[j, i].transform.SetParent(inventoryBody, true);
            }
        }
    }

    public void setBackground(Sprite backgroundSprite)
    {
        background.sprite = backgroundSprite;
    }

    private void add(InventoryItem item, Vector2Int position)
    {
        item.position = position;

        items.Add(item);
        addItemToContents(item);
        item.transform.SetParent(itemParent);
        item.transform.localPosition = new Vector3(position.x * InventoriesManager.instance.gridSize,
            position.y * InventoriesManager.instance.gridSize, 0f);
    }

    public void remove(InventoryItem item)
    {
        item.position = Vector2Int.zero;

        items.Remove(item);
        removeItemFromContents(item);
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
