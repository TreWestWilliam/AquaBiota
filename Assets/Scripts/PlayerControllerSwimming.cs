using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerSwimming : MonoBehaviour
{
    [Header("Player Variables")]
    public PlayerInput _PlayerInput;
    public Rigidbody _Rigidbody;
    public float Movespeed = 25;
    public float VerticalMovespeed = 10;

    [Header("Camera Controls")]
    public Camera PlayerCamera;
    public float CameraDistance = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 MovementInput = _PlayerInput.actions["Move"].ReadValue<Vector2>();
        MovementInput *= Time.deltaTime;
        MovementInput *= Movespeed;
        _Rigidbody.AddForce(transform.forward * MovementInput.y + transform.right * MovementInput.x);
        _Rigidbody.MoveRotation( Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,PlayerCamera.transform.rotation.eulerAngles.y,0 ), .2f ));

        Vector2 CameraInput = _PlayerInput.actions["Look"].ReadValue<Vector2>();
        PlayerCamera.transform.position += PlayerCamera.transform.right * CameraInput.x * Time.deltaTime + PlayerCamera.transform.up * CameraInput.y * Time.deltaTime;
        PlayerCamera.transform.LookAt(transform);
        PlayerCamera.transform.position = transform.position - PlayerCamera.transform.forward * CameraDistance;


    }

    public void SwimUp(CallbackContext callbackContext) 
    {
        //TODO COOLDOWNS? (Probably through coroutines or something)
        _Rigidbody.AddForce(transform.up *VerticalMovespeed + (transform.forward * Movespeed));
    }

    public void SwimDown(CallbackContext callback) 
    {
        _Rigidbody.AddForce(-transform.up *VerticalMovespeed + (transform.forward * Movespeed));
    }

    public void Move(CallbackContext callbackContext) 
    {
        //Vector2 Input = callbackContext.ReadValue<Vector2>();
        //Debug.Log($"MOVE INPUT SANITY TEST!  X: {Input.x}, Y: {Input.y}");
    }

    public void MoveCamera( CallbackContext callbackContext) 
    {/*
        Vector2 Input = callbackContext.ReadValue<Vector2>();
        //Debug.Log($"CAMERA INPUT SANITY TEST!  X: {Input.x}, Y: {Input.y}");

        PlayerCamera.transform.position += PlayerCamera.transform.right * Input.x *Time.deltaTime + PlayerCamera.transform.up * Input.y *Time.deltaTime;
        PlayerCamera.transform.LookAt(transform);
        PlayerCamera.transform.position = transform.position - PlayerCamera.transform.forward * CameraDistance;
        */
    }
}
