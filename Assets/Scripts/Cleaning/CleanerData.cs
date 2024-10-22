using UnityEngine;

[CreateAssetMenu(fileName = "CleanerData", menuName = "Data Objects/CleanerData")]
public class CleanerData : BasicData
{
    [SerializeField] private Mesh cMesh;
    public Mesh mesh
    {
        get
        {
            return cMesh;
        }
    }
    [SerializeField] private Material cMaterial;
    public Material material
    {
        get
        {
            return cMaterial;
        }
    }
    [SerializeField] private Color cColor;
    public Color color
    {
        get
        {
            return cColor;
        }
    }

    [SerializeField] private float WeightPerAmmount;
    public float weightPerAmmount
    {
        get
        {
            return WeightPerAmmount;
        }
    }
}
