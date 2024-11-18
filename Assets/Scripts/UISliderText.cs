using UnityEngine;
using TMPro;
using System;

public class UISliderText : MonoBehaviour
{
    private TMP_Text _Text;

    public void Start()
    {
        _Text = GetComponent<TMP_Text>();
    }
    //Reference for formatting this style of string interpolation:
    //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
    public void SliderChange(float val) {
        if(_Text!=null)
            _Text.text = $"{val:F2}";
    }
}
