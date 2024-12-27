using UnityEngine;

public class OpenWebsite : MonoBehaviour
{
    public static void OpenSite(string web) 
    {
        Application.OpenURL(web);
    }
}
