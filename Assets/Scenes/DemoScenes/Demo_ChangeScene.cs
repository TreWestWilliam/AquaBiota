using UnityEngine;
using UnityEngine.SceneManagement;

public class Demo_ChangeScene : MonoBehaviour
{
    public string SceneName;
    public void SwapScene() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadSceneAsync(SceneName);
    }
}
