using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [Header("Camera Controls")]
    public Camera PlayerCamera;
    public PlayerInput _PlayerInput;
    public GameObject Target;
    public float CameraDistance = 10;
    public float CameraSensitivityVertical = 5f;
    public float CameraSensitivityHorizontal = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerCamera == null ) { PlayerCamera= this.GetComponent<Camera>(); }
        if (_PlayerInput == null) { Debug.Log("Camera's PlayerInput isnt set"); _PlayerInput = GameObject.FindAnyObjectByType<PlayerInput>(); }
        if (Target == null) { Debug.Log("Camera has no target object."); }

    }

    // Update is called once per frame
    void Update()
    {
        //Basic orbiting camera
        Vector2 CameraInput = _PlayerInput.actions["Look"].ReadValue<Vector2>();
       

        transform.position += PlayerCamera.transform.right * CameraInput.x * CameraSensitivityHorizontal * Time.fixedDeltaTime + PlayerCamera.transform.up * CameraInput.y * CameraSensitivityVertical * Time.fixedDeltaTime;
        transform.LookAt(Target.transform);
        transform.position = Target.transform.position - PlayerCamera.transform.forward * CameraDistance;
    }

    public void LoadSettings(Settings s) 
    {
        CameraSensitivityVertical = s.CameraV;

        if (s.CameraVInverse) { CameraSensitivityVertical *= -1; }

        CameraSensitivityHorizontal = s.CameraH;
        if (s.CameraHInverse) { CameraSensitivityHorizontal *= -1; }
    } 
}
