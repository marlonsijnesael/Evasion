using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCollider : MonoBehaviour
{
    public LayerMask collisionMask;

    public BoxCollider col;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public delegate void OnHorizontalCollision(RaycastHit _targetHit);
    public static event OnHorizontalCollision OnCollisionRegistered;

    private RaycastOrigins raycastOrigins;
    public Transform cam;
    public Transform foot;

    private RaycastHit hitInfo;
    [SerializeField] private float padding = 0.05f;
    [SerializeField] private float height = 0.5f;
    public LayerMask ground;
    public bool isGrounded;

    public bool frontCollision = false;

    private void Start()
    {

        CalculateRaySpacing();
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        CheckGround();
        Vector3 velocity = PlayerMove.velocity;
        HorizontalCollisions(ref velocity);
    }

    private void CheckGround()
    {
        if (Physics.Raycast(foot.transform.position, Vector3.down, out hitInfo, height + padding, ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }


    void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }



    private void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = velocity.z;
        float rayLength = 3 + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += cam.right * (horizontalRaySpacing * i);

            RaycastHit hit;

            Debug.DrawRay(rayOrigin, cam.forward * directionX * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, cam.forward * directionX * rayLength, out hit, rayLength, collisionMask))
            {
                OnCollisionRegistered(hit);
            }
            else
            {
                frontCollision = false;
            }
        }
    }

    private void VerticalCollision(ref Vector3 velocity)
    {

    }

}


