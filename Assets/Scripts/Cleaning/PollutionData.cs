using UnityEngine;

[CreateAssetMenu(fileName = "PollutionData", menuName = "Data Objects/PollutionData")]
public class PollutionData : BasicData
{
    [SerializeField] private Mesh pMesh;
    [SerializeField] private Material pMaterial;

    [SerializeField] private Cleaner[] cleaners;

    [SerializeField] private int EnvironmentalImpact;
    public int envImp
    {
        get
        {
            return EnvironmentalImpact;
        }
    }

    public int getCleanerEfectiveness(CleanerData cleaner)
    {
        int ammount = 0;
        if(cleaners[0].cleanerData == null)
        {
            ammount = cleaners[0].ammount;
        }

        foreach(Cleaner type in cleaners)
        {
            if(type.cleanerData == cleaner)
            {
                return type.ammount;
            }
        }

        return ammount;
    }
}
