using TMPro;
using UnityEngine;

public class GraphicSetting : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public void getDropdownValue() {
        int val = dropdown.value;
        Debug.Log(val);
    }
}
