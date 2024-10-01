/// Player Controller Swimming by Hachiski
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerSwimming : MonoBehaviour
{
    public Player player;

    [Header("Player Variables")]
    public PlayerInput _PlayerInput;
    public Rigidbody _Rigidbody;
    public float Movespeed = 25;
    public float VerticalMovespeed = 10;
    public float RotationalSpeed = 5;
    public float SwimBoostSpeed = 20;

    [Header("Camera Controls")]
    public Camera PlayerCamera;
    public float CameraDistance = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock the cursor on start for mouse+keyboard input
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 MovementInput = _PlayerInput.actions["Move"].ReadValue<Vector2>();
        MovementInput *= Time.deltaTime * Movespeed;
        
        _Rigidbody.AddForce(transform.forward * MovementInput.magnitude);
        // Our rotation is basically the Camera's Rotation + The Input Direction Linerally Interpolated from our previous rotation
        _Rigidbody.MoveRotation( Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0,(PlayerCamera.transform.rotation.eulerAngles.y - Vector2.SignedAngle(Vector2.up, MovementInput)),0)  ), RotationalSpeed  * MovementInput.magnitude));


        //Basic orbiting camera
        Vector2 CameraInput = _PlayerInput.actions["Look"].ReadValue<Vector2>();
        PlayerCamera.transform.position += PlayerCamera.transform.right * CameraInput.x * Time.deltaTime + PlayerCamera.transform.up * CameraInput.y * Time.deltaTime;
        PlayerCamera.transform.LookAt(transform);
        PlayerCamera.transform.position = transform.position - PlayerCamera.transform.forward * CameraDistance;
    }
    // We may wish to adjust the forward momentum gain in the future since it's quite significant
    public void SwimUp(CallbackContext callbackContext) 
    {
        //TODO COOLDOWNS? (Probably through coroutines or something)
        _Rigidbody.AddForce(transform.up *VerticalMovespeed + (transform.forward * SwimBoostSpeed));
    }

    public void SwimDown(CallbackContext callback) 
    {
        _Rigidbody.AddForce(-transform.up *VerticalMovespeed + (transform.forward * SwimBoostSpeed));
    }

    public void Interact(CallbackContext callbackContext)
    {
        if(callbackContext.started)
        {
            player.Interact();
        }
    }

    public void onControlsChanged(PlayerInput input)
    {
        if(input.currentControlScheme.Equals("Gamepad"))
        {
            InputDevice device = input.GetDevice<InputDevice>();
            if(device is DualShockGamepad)
            {
                player.setControllerUI(controllerType.PlayStation);
            }
            else if(device is SwitchProControllerHID)
            {
                player.setControllerUI(controllerType.Switch);
            }
            else
            {
                player.setControllerUI(controllerType.xBox);
            }
        }
        else
        {
            player.setControllerUI(controllerType.MouseAndKeyboard);
        }
    }

    //These functions were from testing, didn't work out since it only moved whenver the controller changed directions
    public void Move(CallbackContext callbackContext) 
    {
    }

    public void MoveCamera( CallbackContext callbackContext) 
    {
    }
}
