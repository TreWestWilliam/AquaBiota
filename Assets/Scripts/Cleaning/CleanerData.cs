using UnityEngine;

[CreateAssetMenu(fileName = "CleanerData", menuName = "Data Objects/CleanerData")]
public class CleanerData : ScriptableObject
{
    [SerializeField] private string CleanerName;
    public string cleanerName
    {
        get
        {
            return CleanerName;
        }
    }

    [SerializeField] private Mesh cMesh;
    [SerializeField] private Material cMaterial;
    [SerializeField] private Color cColor;

    [SerializeField] private int UsePerSpray;

    [SerializeField] private float WeightPerAmmount;

    [SerializeField] private float Range;
    [SerializeField] private float Spread;
}
