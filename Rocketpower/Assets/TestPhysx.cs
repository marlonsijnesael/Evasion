using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysx : MonoBehaviour
{

    [Header("Collision settings: ")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private BoxCollider col;
    [SerializeField] private float maxAngle = 65f;
    [SerializeField] private int horizontalRayCount = 4;
    [SerializeField] private int verticalRayCount = 4;
    [SerializeField] private float padding = 0.05f;
    [SerializeField] private float height = 0.50f;

    [Header("Scene objects: ")]
    public Transform head;
    public Transform body;

    [Header("Debug variables: ")]
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
    public RaycastHit leftHit;
    public RaycastHit rightHit;
    private bool wallrun;
    public AnimContrller.animations animationss;
    private void Start()
    {
        CalculateRaySpacing();
        gravity = (2 * jumpheight) / (timeToJumpApex * timeToJumpApex);
        jumpVelocity = gravity * timeToJumpApex;
    }

    public void Act()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            Coroutine coroutine = StartCoroutine(Jump());
        }
        if (!overrideForce)
        {
            UpdateRaycastOrigins();
            ApplyGravity();
            Vector3 velocity = TestMove.velocity;
            HorizontalCollisions(ref velocity);
            VerticalCollisions(ref velocity);
            LeftRightCollisions();
        }
    }

    private void UpdateRaycastOrigins()
    {
        raycastOrigins.topLeft = new Vector3(col.bounds.min.x, col.bounds.max.y, col.bounds.max.z);
        raycastOrigins.topRight = new Vector3(col.bounds.max.x, col.bounds.max.y, col.bounds.max.z);
    }

    private void ApplyGravity()
    {
        if (!isGrounded && !jumping && !overrideForce)
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
        var bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.x / (horizontalRayCount);
        verticalRaySpacing = bounds.size.x / (verticalRayCount);
    }

    private void LeftRightCollisions()
    {
        if (Physics.Raycast(transform.position, transform.right, out rightHit, 1 + skinWidth))
        {
            if (Vector3.Dot(rightHit.normal, Vector3.up) == 0)
            {
                Debug.Log("right wall" + rightHit.transform.CompareTag("wall"));

                Vector3 cross = Vector3.Cross(rightHit.normal, Vector3.up);
                Debug.DrawRay(rightHit.point, cross, Color.red, 10f);


            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out leftHit, 1 + skinWidth))
        {
            if (Vector3.Dot(leftHit.normal, Vector3.up) == 0)
            {
                wallrun = true;
                Debug.Log("left wall" + leftHit.transform.CompareTag("wall"));
                Debug.DrawLine(leftHit.normal, leftHit.normal, Color.red, 10f);
            }
        }

    }




    private void HorizontalCollisions(ref Vector3 velocity)
    {

        int half = verticalRayCount / 2;
        for (int i = -half; i < half; i++)
        {
            Vector3 rayOrigin = transform.position;
            rayOrigin += transform.right * (horizontalRaySpacing * i);

            Debug.DrawRay(rayOrigin, transform.forward, Color.blue);

            if (!Physics.Raycast(rayOrigin, transform.forward, out horizontalHit, 1 + skinWidth, collisionMask))
            {
                stopForce = Vector3.zero;
                horizontalStop = false;
            }

            else
            {
                float angle = Vector3.Angle(horizontalHit.normal, body.transform.up);
                if (angle > maxAngle)
                {
                    stopForce = velocity;
                    horizontalStop = true;
                }
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
            Vector3 rayOrigin = transform.position;
            rayOrigin += transform.right * (verticalRaySpacing * i);

            Debug.DrawRay(rayOrigin, -transform.up, Color.red);
            Debug.DrawRay(head.transform.position, head.transform.forward, Color.green);
            if (!Physics.Raycast(rayOrigin, -transform.up, out verticalHit, height + padding, collisionMask))
            {
                isGrounded = false;
                GetComponent<PlayerManager>().SetAnimation("grounded", false);
            }
            else
            {
                GetComponent<PlayerManager>().SetAnimation("grounded", true);
                if (verticalHit.transform.CompareTag("Action"))
                {
                    verticalHit.collider.GetComponent<PhysicsAction>().Act(this);
                }
                isGrounded = true;
            }
        }
    }

    public IEnumerator Jump()
    {
        // jumping = true;
        // gravity = 2 * jumpheight / Mathf.Pow(timeToJumpApex, 2);
        // jumpVelocity = gravity * timeToJumpApex;
        // upwardForce = new Vector3(0, jumpVelocity, 0);

        // //yield return new WaitForSeconds(timeToJumpApex);
        // while (upwardForce.y > 0)
        // {
        //     upwardForce.y -= Physics.gravity.y * Time.deltaTime;
        // }
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



