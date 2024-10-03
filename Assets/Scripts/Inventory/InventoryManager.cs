using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryGrid;

    [SerializeField] private int rows;
    [SerializeField] private int columns;
    private static int maxRows = 10;
    private static int maxColumns = 10;

    public GameObject[,] inventorySlots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventorySlots = new GameObject[maxRows, maxColumns];
        inventoryGrid.GetComponent<GridLayoutGroup>().constraintCount = columns;

        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                inventorySlots[i, j] = Instantiate(inventorySlotPrefab);
                inventorySlots[i, j].transform.SetParent(inventoryGrid.transform, true);
            }
        }
    }

    // Sets inventory to new number of rows and columns
    public void UpdateInventory(int newRows, int newColumns)
    {
        inventoryGrid.GetComponent<GridLayoutGroup>().constraintCount = newColumns;

        while (rows > newRows)
        {
            rows--;
            for (int i = 0; i < columns; i++)
            {
                Destroy(inventorySlots[rows, i]);
                inventorySlots[rows, i] = null;
            }
        }
        while (rows < newRows)
        {
            for (int i = 0; i < columns; i++)
            {
                inventorySlots[rows, i] = Instantiate(inventorySlotPrefab);
                inventorySlots[rows, i].transform.SetParent(inventoryGrid.transform, true);
            }
            rows++;
        }

        while (columns > newColumns)
        {
            columns--;
            for (int i = 0; i < rows; i++)
            {
                Destroy(inventorySlots[i, columns]);
                inventorySlots[i, columns] = null;
            }
        }
        while (columns < newColumns)
        {
            for (int i = 0; i < rows; i++)
            {
                inventorySlots[i, columns] = Instantiate(inventorySlotPrefab);
                inventorySlots[i, columns].transform.SetParent(inventoryGrid.transform, true);
            }
            columns++;
        }
    }
}
