using NUnit.Framework;
using UnityEngine;

public class ItemObject : MonoBehaviour, InteractableObject
{
    public ItemData itemData;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private bool aboveSurface = false;

    public Inventory contents;

    public InventoryItem inventoryItem;

    public string interactDescription = "Pick Up";

    public float weight
    {
        get
        {
            float totalWeight = itemData.weight;
            if(itemData.isContainer && contents != null)
            {
                totalWeight += contents.getWeight();
            }
            return totalWeight;
        }
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        rb.mass = itemData.weight;
        rb.linearDamping = itemData.drag;
    }

    private void Start()
    {
        if(inventoryItem != null && inventoryItem.transform.parent == transform)
        {
            inventoryItem.transform.SetParent(InventoriesManager.instance.looseItemParent);
        }

        if(itemData.isContainer && contents == null)
        {

        }
    }

    private void FixedUpdate()
    {
        /*if(itemData.buoyancy > 0f)
        {
            Debug.Log(gameObject.name + " is moving " + rb.linearVelocity.y + " at " + transform.position.y);
        }*/
        if(transform.position.y < OceanManager.instance.lowestWaveHeight() || transform.position.y < OceanManager.instance.waveHeight(transform.position.x, transform.position.y, Time.time))
        {
            if(aboveSurface)
            {
                //Debug.Log(gameObject.name + " submerged.");
                rb.AddForce(Vector3.down * rb.linearVelocity.y * rb.mass * 0.2f, ForceMode.Impulse);
                aboveSurface = false;
            }
            rb.AddForce(Vector3.up * itemData.buoyancy * 5);
        }
        else
        {
            if(!aboveSurface)
            {
                //Debug.Log(gameObject.name + " surfaced.");
                rb.AddForce(Vector3.down * rb.linearVelocity.y * rb.mass * 0.6f, ForceMode.Impulse);
                aboveSurface = true;
            }
            rb.AddForce(Vector3.down * itemData.weight * 5);
        }
    }

    public string getInteractText()
    {
        return interactDescription + "\n" + itemData.dataName;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public Transform getTransform()
    {
        return transform;
    }

    public void beginInteraction(Player player)
    {
        player.openInventory(inventoryItem);
    }

    public void endInteraction(Player player, bool confirmed)
    {
        if(confirmed)
        {
            if(itemData.pickupSound != null)
            {
                AudioManager.instance.playSFXAudioClip(itemData.pickupSound, transform, 1f);
            }
            gameObject.SetActive(false);
        }
    }
}
