using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public Textbox[] textbox; 
}
[System.Serializable]
public class Textbox
{
    public string name;

    [TextArea(1, 10)]
    public string sentence;
}