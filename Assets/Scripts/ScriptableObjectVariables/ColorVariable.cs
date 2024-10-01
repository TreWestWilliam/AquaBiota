using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Variables/Color", order = 1)]
public class ColorVariable : ScriptableObject
{
    public Color Value = Color.white;
}