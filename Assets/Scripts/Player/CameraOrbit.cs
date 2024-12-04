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

    public float MaxXRot = 85;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (PlayerCamera == null) { PlayerCamera = this.GetComponent<Camera>(); }
        if (_PlayerInput == null) { Debug.Log("Camera's PlayerInput isnt set"); _PlayerInput = GameObject.FindAnyObjectByType<PlayerInput>(); }
        if (Target == null) { Debug.Log("Camera has no target object."); }
        LoadSettings(Options.Instance._Settings);
    }

    // Update is called once per frame
    private void Update()
    {
        //Basic orbiting camera
        Vector2 CameraInput = _PlayerInput.actions["Look"].ReadValue<Vector2>();

        transform.position += PlayerCamera.transform.right * CameraInput.x * CameraSensitivityHorizontal * Time.fixedDeltaTime + PlayerCamera.transform.up * CameraInput.y * CameraSensitivityVertical * Time.fixedDeltaTime;
        transform.LookAt(Target.transform);

        //This is a simple /imperfect fix to prevent weird issues when the camera is overlapping the player aka when the camera would be at 90 degrees on the X axis.
        float rotcheck = transform.localEulerAngles.x;
        if (rotcheck > 90) { rotcheck -= 360; }

        if (MaxXRot < rotcheck || -MaxXRot > rotcheck)
        {
            Debug.Log(rotcheck);
            //Debug.Log($"Positive/Negative={posNeg}");
            float adjustmentVal = Mathf.Abs(rotcheck) - MaxXRot;
            float posNeg = rotcheck / Mathf.Abs(rotcheck);

            transform.position += PlayerCamera.transform.up * adjustmentVal * -posNeg;
            transform.LookAt(Target.transform);
            //transform.position = Target.transform.position - PlayerCamera.transform.forward * CameraDistance;
        }
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