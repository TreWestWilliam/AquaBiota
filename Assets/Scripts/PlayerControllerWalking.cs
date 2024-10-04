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
    public PhysicsMaterial WalkingMaterial;
    public PhysicsMaterial StoppedMaterial;
    private CapsuleCollider _CapsuleCollider;
    [SerializeField] private bool IsGrounded;

    [Header("Animation")]
    public Animator _Animator;

    [Header("Camera Controls")]
    public Camera PlayerCamera;
    public float CameraDistance = 10;
    public bool DebugMode = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock the cursor on start for mouse+keyboard input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _CapsuleCollider = GetComponent<CapsuleCollider>();
        _Animator = (_Animator != null) ? _Animator = _Animator : _Animator = GetComponentInChildren<Animator>(); 

    }

    // Update is called once per frame
    void Update()
    {
        // Primary Ground Ray tells the controller if it's grounded for jumping
        Ray GroundingRay = new( transform.position, -transform.up );
        RaycastHit GroundingRayInfo;
        Vector3 GroundNormal = Vector3.zero; ;
        if (Physics.Raycast(transform.position, -transform.up, out GroundingRayInfo, 1.35f))
        {
            GroundNormal = GroundingRayInfo.normal;
            GroundNormal = new Vector3(0, (1-GroundNormal.y), 0);
            IsGrounded = true;
        }
        else { IsGrounded = false; }
        //Slope ray is here to see if there's a slope infront of the player, and enables us to climb it better by adding upwards momentum.
        RaycastHit SlopeRay;
        if (Physics.Raycast(transform.position, -transform.up + transform.forward, out SlopeRay, 1.5f)) 
        {
            Debug.Log($"SlopeRay: {SlopeRay.normal}");
            GroundNormal = new Vector3(0, GroundNormal.y + (1-SlopeRay.normal.y), 0);
        }
        // Debug ray to show where the slope ray is pointing
        if (DebugMode)
            Debug.DrawRay(transform.position, -transform.up + transform.forward, Color.green);

        Vector2 MovementInput = _PlayerInput.actions["Move"].ReadValue<Vector2>();

        //If the player stops trying to move we use the stopped material to make the player slow down drastically.
        _CapsuleCollider.material = (MovementInput.magnitude > 0.1f) ? WalkingMaterial : StoppedMaterial;


        MovementInput *= Time.deltaTime * Movespeed;

        float ClimbMultiplier = 1;
        if ((transform.forward + GroundNormal).normalized.y > .2f  && (transform.forward + GroundNormal).normalized.y < .65f) { ClimbMultiplier = 1.8f; }
        else if ((transform.forward + GroundNormal).normalized.y > .7f) { ClimbMultiplier = .2f; }

        _Rigidbody.AddForce( (transform.forward + GroundNormal).normalized * (MovementInput.magnitude * ClimbMultiplier) );

        if (DebugMode)
        {
            // Shows the direction the player is headed
            Debug.DrawRay(transform.position, (transform.forward + GroundNormal).normalized, Color.red);
            // number output for direction
            Debug.Log($"{transform.forward} + {GroundNormal}  = {(transform.forward + GroundNormal.normalized).normalized}");
        }

        // Our rotation is basically the Camera's Rotation + The Input Direction Linerally Interpolated from our previous rotation
        _Rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, (PlayerCamera.transform.rotation.eulerAngles.y - Vector2.SignedAngle(Vector2.up, MovementInput)), 0)), RotationalSpeed * MovementInput.magnitude * (_Rigidbody.linearVelocity.magnitude/Movespeed) + (MovementInput.magnitude *.01f) ));


        //Animation stuff here
        bool walking = (MovementInput.magnitude > 0);
        //todo:sprinting?  maybe idk movement speed is good enough for a small world.
        _Animator.SetBool("Walking", walking);
        float AnimSpeed=  Mathf.Max(.1f, MovementInput.magnitude);
        _Animator.SetFloat("Movespeed", AnimSpeed);
        //Todo: Jump animations

        //Basic orbiting camera
        Vector2 CameraInput = _PlayerInput.actions["Look"].ReadValue<Vector2>();
        PlayerCamera.transform.position += PlayerCamera.transform.right * CameraInput.x * Time.deltaTime + PlayerCamera.transform.up * CameraInput.y * Time.deltaTime;
        PlayerCamera.transform.LookAt(transform);
        PlayerCamera.transform.position = transform.position - PlayerCamera.transform.forward * CameraDistance;


    }

    public void Jump(CallbackContext callbackContext) 
    {
        if (IsGrounded)
            _Rigidbody.AddForce(JumpPower * transform.up);
    }
}
