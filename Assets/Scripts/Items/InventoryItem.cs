using Unity.VectorGraphics;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    public Vector2Int position;

    public SVGImage image;

    [SerializeField] private Rotation itemRotation;
    public Rotation rotation
    {
        get
        {
            return itemRotation;
        }
    }

    [SerializeField] private Vector2Int currentLocation;

    public Inventory contents;

    public ItemObject itemObject;

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

    public Vector2Int getLocation()
    {
        return currentLocation;
    }

    #region Rotation
    public void setRotation(Rotation rotation)
    {
        itemRotation = rotation;
        float x = 0f;
        float z;
        if(rotation.flipped())
        {
            x = 180f;
        }

        switch(rotation.unFlippedRotation())
        {
            case Rotation.Left:
                z = 90f;
                break;
            case Rotation.Right:
                z = -90f;
                break;
            case Rotation.Down:
                z = 180f;
                break;
            default:
                z = 0f;
                break;
        }
        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = new Vector3(x, 0f, z);

        Vector3 imagePosition = image.rectTransform.localPosition;
        if(rotation.onSide())
        {
            imagePosition = new Vector3(-imagePosition.y, -imagePosition.x, imagePosition.z);
        }

        image.rectTransform.SetLocalPositionAndRotation(imagePosition, quaternion);
    }

    public void rotateClockwise()
    {
        setRotation(rotation.clockwise());
    }

    public void rotateWiddershins()
    {
        setRotation(rotation.widdershins());
    }

    public void flipHorizontal()
    {
        setRotation(rotation.flipHorizontal());
    }

    public void flipVertical()
    {
        setRotation(rotation.flipVertical());
    }
    #endregion
}
