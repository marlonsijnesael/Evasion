using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysx : MonoBehaviour
{

    [Header("Collision settings: ")]
    [SerializeField] private  LayerMask collisionMask;
    [SerializeField] private BoxCollider col;
    [SerializeField] private float maxAngle = 65f;
    [SerializeField] private int horizontalRayCount = 4;
    [SerializeField] private int verticalRayCount = 4;
    [SerializeField] private float padding = 0.05f;
    [SerializeField] private float height = 0.5f;
    
    [Header("Scene objects: ")]
    public Transform parent;
    public Transform cam;
    public Transform foot;
    
    [Header ("Debug variables: ")]
    public bool isGrounded = false;
    public bool horizontalStop = false;
    public bool jumping = false;
    public bool overrideForce = false;
    public Vector3 downwardForce = Vector3.zero;
    public Vector3 stopForce = Vector3.zero;
    public Vector3 upwardForce = Vector3.zero;


    [Header("Jump settings: ")]
    public float jumpheight = 5;
    public float timeToJumpApex = 2f;
    public float gravity;
    public float jumpVelocity;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private RaycastOrigins raycastOrigins;
    const float skinWidth = .015f;
    public RaycastHit verticalHit;
    public RaycastHit horizontalHit;

    private void Start()
    {
        CalculateRaySpacing();
        gravity = (2 * jumpheight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = gravity * timeToJumpApex;
    }

    private void FixedUpdate()
    {
        UpdateRaycastOrigins();
        ApplyGravity();
        Vector3 velocity = TestMove.velocity;
        HorizontalCollisions(ref velocity);
        VerticalCollisions(ref velocity);
        if (Input.GetKeyDown(KeyCode.Space))
        {
         StartCoroutine( Jump()); 
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
    }

    private void ApplyGravity()
    {
        if (!isGrounded && !jumping)
        {
            downwardForce += Physics.gravity * Time.deltaTime;
        }
        else
        {
            downwardForce = Vector3.zero;
        }
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.x / (horizontalRayCount);
        verticalRaySpacing = bounds.size.x / (verticalRayCount);
    }

    private void HorizontalCollisions(ref Vector3 velocity)
    {

        float rayLength = 1 + skinWidth;
        int half = verticalRayCount / 2;
        for (int i = -half; i < half; i++)
        {
            Vector3 rayOrigin = foot.position;
            rayOrigin += foot.transform.right * (horizontalRaySpacing * i);

            Debug.DrawRay(rayOrigin, foot.forward, Color.red);

            if (Physics.Raycast(rayOrigin, foot.forward, out horizontalHit, rayLength, collisionMask))
            {
                float angle = Vector3.Angle(horizontalHit.normal, foot.transform.up);
                if (angle > maxAngle)
                {
                    stopForce = velocity;
                    horizontalStop = true;
                }
            }

            else
            {
                stopForce = Vector3.zero;
                horizontalStop = false;
            }
        }
    }

    private void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        int half = verticalRayCount / 2;
        for (int i = -half; i < half; i++)
        {
            Vector3 rayOrigin = foot.transform.position;
            rayOrigin += foot.transform.right * (verticalRaySpacing * i);

            Debug.DrawRay(rayOrigin, -foot.up, Color.red);

            if (Physics.Raycast(rayOrigin, -foot.up, out verticalHit, height + padding, collisionMask))
            {
                if (verticalHit.transform.CompareTag("Action")) {
                    verticalHit.collider.GetComponent<PhysicsAction>().Act(this);
                        }
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    public IEnumerator Jump()
    {
        jumping = true;
        gravity = 2 * jumpheight / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = gravity * timeToJumpApex;
        upwardForce = new Vector3(0, jumpVelocity, 0);
      
        yield return new WaitForSeconds(timeToJumpApex);
        while (upwardForce.y > 0)
        {
            upwardForce.y -= gravity * Time.deltaTime;
        }
        jumping = false;
        upwardForce = Vector3.zero;
        yield return null;
    }

}

struct RaycastOrigins
{
    public Vector3 topLeft, topRight;
    public Vector3 bottomLeft, bottomRight;

    public Vector3 bottom_front_left, bottom_front_right;
    public Vector3 bottom_back_left, bottom_back_right;
}



