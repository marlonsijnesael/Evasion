using UnityEngine;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class PlayerRotation : MonoBehaviour
{
    public Transform playerCam, player;
    public Vector3 lastDir;
    float turnSmoothing = -.25f;
    public VirtualController vC;


    public float walkSpeed = 0.15f;                 // Default walk speed.
    public float runSpeed = 1.0f;                   // Default run speed.
    public float sprintSpeed = 2.0f;                // Default sprint speed.
    public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
    public string jumpButton = "Jump";              // Default jump button.
    public float jumpHeight = 1.5f;                 // Default jump height.
    public float jumpIntertialForce = 10f;          // Default horizontal inertial force when jumping.

    private float speed, speedSeeker;               // Moving speed.
    private int jumpBool;                           // Animator variable related to jumping.
    private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
    private bool jump;                              // Boolean to determine whether or not the player started a jump.
    private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.

    // Start is always called after any Awake functions.
    void Start()
    {
        lastDir = transform.forward;
    }

  

    // LocalFixedUpdate overrides the virtual function of the base class.
    public  void FixedUpdate()
    {
        Rotating();
    }



    // Rotate the player to match correct orientation, according to camera and key pressed.
    Vector3 Rotating()
    {
        // Get camera forward direction, without vertical component.
        Vector3 forward = playerCam.TransformDirection(Vector3.forward);

        // Player is moving on ground, Y component of camera facing is not relevant.
        forward.y = 0.0f;
        forward = forward.normalized;

        // Calculate target direction based on camera forward and direction key.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        targetDirection = forward * vC.VerticalMovement + right * vC.HorizontalMovement;

        // Lerp current direction to calculated target direction.
        if ((vC.VerticalMovement != 0 || vC.HorizontalMovement != 0) && targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
           transform.rotation = newRotation;
            lastDir = targetDirection;
        }
        // If idle, Ignore current camera facing and consider last moving direction.
        if (!(Mathf.Abs(vC.VerticalMovement) > 0 || Mathf.Abs(vC.HorizontalLook) > 0))
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastDir);

            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
           transform.rotation = newRotation;
        }

        return targetDirection;
    }
}
