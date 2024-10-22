using UnityEngine;

[CreateAssetMenu(fileName = "SprayerData", menuName = "Data Objects/SprayerData")]
public class SprayerData : BasicData
{
    [SerializeField] private int AmmountPerSpray;
    public int ammountPerSpray
    {
        get
        {
            return AmmountPerSpray;
        }
    }

    [SerializeField] private float SprayDuration;
    public float sprayDuration
    {
        get
        {
            return SprayDuration;
        }
    }
    [SerializeField] private float ConsecutiveSprayDelay;
    public float consecutiveSprayDelay
    {
        get
        {
            return ConsecutiveSprayDelay;
        }
    }

    [SerializeField] private float Range;
    public float range
    {
        get
        {
            return Range;
        }
    }
    [SerializeField] private float Spread;
    public float spread
    {
        get
        {
            return Spread;
        }
    }

    [SerializeField] private float Weight;
    public float weight
    {
        get
        {
            return Weight;
        }
    }
}