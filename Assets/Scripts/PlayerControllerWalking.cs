using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerControllerWalking : MonoBehaviour
{
    [Header("Player Variables")]
    public PlayerInput _PlayerInput;
    public Rigidbody _Rigidbody;
    public float Movespeed = 25;
    public float VerticalMovespeed = 10;
    public float RotationalSpeed = 5;
    public float SwimBoostSpeed = 20;
    public float JumpPower = 40;
    [SerializeField] private bool IsGrounded;

    [Header("Camera Controls")]
    public Camera PlayerCamera;
    public float CameraDistance = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock the cursor on start for mouse+keyboard input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        // Primary Ground Ray tells the controller if it's grounded for jumping
        Ray GroundingRay = new( transform.position, -transform.up );
        RaycastHit GroundingRayInfo;
        Vector3 GroundNormal = Vector3.zero; ;
        if (Physics.Raycast(transform.position, -transform.up, out GroundingRayInfo, 1.15f))
        {
            GroundNormal = GroundingRayInfo.normal;
            GroundNormal = new Vector3(0, Mathf.Abs(GroundNormal.y - 1), 0);
            IsGrounded = true;
        }
        else { IsGrounded = false; }
        //Slope ray is here to see if there's a slope infront of the player, and enables us to climb it better by adding upwards momentum.
        RaycastHit SlopeRay;
        if (Physics.Raycast(transform.position, -transform.up + transform.forward, out SlopeRay, 1.5f)) 
        {
            Debug.Log($"SlopeRay: {SlopeRay.normal}");
            GroundNormal = new Vector3(0, GroundNormal.y + Mathf.Abs(SlopeRay.normal.y - 1), 0);
        }
        Debug.DrawRay(transform.position, -transform.up + transform.forward, Color.green);

        Vector2 MovementInput = _PlayerInput.actions["Move"].ReadValue<Vector2>();
        MovementInput *= Time.deltaTime * Movespeed;

        _Rigidbody.AddForce( (transform.forward + GroundNormal).normalized * MovementInput.magnitude);
        Debug.Log($"{transform.forward} + {GroundNormal}  = {(transform.forward + GroundNormal.normalized).normalized}");
        // Our rotation is basically the Camera's Rotation + The Input Direction Linerally Interpolated from our previous rotation
        _Rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, (PlayerCamera.transform.rotation.eulerAngles.y - Vector2.SignedAngle(Vector2.up, MovementInput)), 0)), RotationalSpeed * MovementInput.magnitude * (_Rigidbody.linearVelocity.magnitude/Movespeed) + (MovementInput.magnitude *.01f) ));


        //Basic orbiting camera
        Vector2 CameraInput = _PlayerInput.actions["Look"].ReadValue<Vector2>();
        PlayerCamera.transform.position += PlayerCamera.transform.right * CameraInput.x * Time.deltaTime + PlayerCamera.transform.up * CameraInput.y * Time.deltaTime;
        PlayerCamera.transform.LookAt(transform);
        PlayerCamera.transform.position = transform.position - PlayerCamera.transform.forward * CameraDistance;


    }

    public void Jump(CallbackContext callbackContext) 
    { 
    
    }
}
