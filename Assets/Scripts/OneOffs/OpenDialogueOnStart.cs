using UnityEngine;

public class OpenDialogueOnStart : MonoBehaviour
{
    public Dialogue d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(Open), .05f);
    }

    private void Open() { MenuManager.Instance.startDialogue(d); }

    // Update is called once per frame
    void Update()
    {
        
    }
}
