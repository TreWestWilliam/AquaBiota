using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string ItemName;
    public string itemName
    {
        get
        {
            return ItemName;
        }
    }

    [SerializeField] private SpriteRef itemSprite;
    public Sprite sprite
    { 
        get
        {
            return itemSprite;
        }
    }

    [SerializeField] private ItemObject ItemObjectPrefab;
    public ItemObject itemObject
    {
        get
        {
            return ItemObjectPrefab;
        }
    }

    [SerializeField] private InventoryItem InventoryItemPrefab;
    public InventoryItem inventoryItem
    {
        get
        {
            return InventoryItemPrefab;
        }
    }

    [SerializeField] private InventorySpace InventoryShape;
    public InventorySpace inventoryShape
    {
        get
        {
            return InventoryShape;
        }
    }


    [SerializeField] private bool IsContainer;
    public bool isContainer
    {
        get
        {
            return IsContainer;
        }
    }

    [SerializeField] private InventorySpace ContentsShape;
    public InventorySpace contentsShape
    {
        get
        {
            return ContentsShape;
        }
    }

    [SerializeField] private SpriteRef ContentsBackground;
    public Sprite contentsBackground
    {
        get
        {
            return ContentsBackground;
        }
    }

    [SerializeField] private FloatRef Weight;
    public float weight
    {
        get
        {
            return Weight;
        }
    }

    [SerializeField] private FloatRef Buoyancy;
    public float buoyancy
    {
        get
        {
            return Buoyancy;
        }
    }

    [SerializeField] private FloatRef Drag;
    public float drag
    {
        get
        {
            return Drag;
        }
    }

    [SerializeField] private IntRef Value;
    public int value
    {
        get
        {
            return Value;
        }
    }

    [SerializeField] private ItemTags[] Tags;
    public ItemTags[] tags
    {
        get
        {
            return Tags;
        }
    }
}
