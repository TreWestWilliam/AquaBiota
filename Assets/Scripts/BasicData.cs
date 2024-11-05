using UnityEngine;

public abstract class BasicData : ScriptableObject
{
    [SerializeField] private string DataName;
    public string dataName
    {
        get
        {
            return DataName;
        }
    }

    [SerializeField] private SpriteRef dataSprite;
    public Sprite sprite
    {
        get
        {
            return dataSprite;
        }
    }

    [TextArea(3, 10)]
    [SerializeField] private string Description;
    public string description
    {
        get
        {
            return Description;
        }
    }
}
