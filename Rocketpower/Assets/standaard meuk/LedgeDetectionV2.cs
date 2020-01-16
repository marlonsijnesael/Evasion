using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetectionV2 : MonoBehaviour
{
    // [SerializeField] CapsuleCollider capsuleCollider;

    // // Layer mask for world geometry. Make sure to ignore the player!
    // [SerializeField] LayerMask geometryMask = ~0;
    // // How high up above root the hands of the character will be
    // [SerializeField] float grabHeight = 1.75f;
    // // How far our horizontal rays go
    // [SerializeField] float horizontalDistanceCheck = 0.75f;
    // // How far our vertical rays go
    // [SerializeField] float verticalDistanceCheck = 0.75f;
    // // How steep the surface above the ledge can be for it to be considered a ledge
    // [SerializeField] float maxSlopeAngle = 20;
    // // Distance of the final capsule cast
    // [SerializeField] float capsuleCastCheckDistance = 0.75f;
    // // Buffer for our nonalloc physics checks
    // Collider[] collisionResults = new Collider[20];

    // private void Start()
    // {
    //     capsuleCollider = GetComponent<CapsuleCollider>();
    // }

    // // private void Update()
    // // {
    // //     print("found ledge: " + TryGetLedgeGrabPoint().empty);
    // // }

    // /// <summary>
    // /// Searches for a ledge in front of this object's transform.
    // /// </summary>
    // /// <param name="ledgePoint">Point on the ledge we want to grab</param>
    // /// <param name="ledgeNormal">The normal of the ledge point</param>
    // /// <returns>Returns whether a ledge was found or not</returns>
    // public Ledge TryGetLedgeGrabPoint()
    // {
    //     // Set our out variables
    //     Vector3 ledgePoint = Vector3.zero;
    //     Vector3 ledgeNormal = Vector3.zero;

    //     // Set our out variables
    //     ledgePoint = Vector3.zero;
    //     ledgeNormal = Vector3.zero;

    //     // Check if the space over the top of our head is empty
    //     int hitCount = Physics.OverlapSphereNonAlloc(
    //         transform.position + new Vector3(0, grabHeight, 0),
    //         capsuleCollider.radius,
    //         collisionResults,
    //         geometryMask,
    //         QueryTriggerInteraction.Ignore
    //     );

    //     // If we hit anything at all, abort
    //     if (hitCount != 0)
    //     {
    //         Debug.DrawLine(transform.position + Vector3.up * grabHeight, Vector3.up, Color.red);
    //         print("no hits");
    //         return new Ledge(Vector3.zero, Vector3.zero, true);

    //     }

    //     print("hit something");
    //     int verticalHits = 0;

    //     // Horizontal rays to check above and over the ledge
    //     for (int i = -1; i <= 1; i++)
    //     {
    //         Vector3 origin = transform.position + new Vector3(0, grabHeight, 0) + transform.right * capsuleCollider.radius * i;
    //         if (Physics.Raycast(origin, transform.forward, horizontalDistanceCheck, geometryMask, QueryTriggerInteraction.Ignore))
    //         {
    //             // Exit if it's not clear
    //             Debug.DrawRay(origin, transform.forward * horizontalDistanceCheck, Color.red);
    //             return new Ledge(Vector3.zero, Vector3.zero, true);

    //         }
    //         else
    //         {
    //             RaycastHit hit;

    //             // if no hit, cast down from the ends of our previous rays

    //             // Move origin to the end of the previous ray
    //             origin += transform.forward * horizontalDistanceCheck;

    //             if (Physics.Raycast(origin, Vector3.down, out hit, verticalDistanceCheck, geometryMask, QueryTriggerInteraction.Ignore))
    //             {
    //                 // If end hits, check slope.
    //                 if (Vector3.Angle(hit.normal, Vector3.up) < maxSlopeAngle)
    //                 {
    //                     Debug.DrawRay(origin, Vector3.down * verticalDistanceCheck, Color.green);
    //                     verticalHits++;
    //                 }
    //                 else
    //                 {
    //                     Debug.DrawRay(origin, Vector3.down * verticalDistanceCheck, Color.red);
    //                 }
    //             }
    //         }
    //     }

    //     // We want at least two hits. 
    //     // This is sort of arbitrary, but one missed hit is usually acceptable.
    //     if (verticalHits < 2)
    //     {
    //         Debug.Log("Hits failed");
    //         return new Ledge(Vector3.zero, Vector3.zero, true);
    //     }

    //     // Capsule cast to find the ledge point and normal

    //     // 1 sphere above the character
    //     Vector3 capsuleTop = transform.position + (capsuleCollider.height + capsuleCollider.radius * 2) * Vector3.up;
    //     // 1 sphere behind the top sphere on the character capsule
    //     Vector3 capsuleBottom = capsuleTop - Vector3.up * capsuleCollider.radius * 2;
    //     capsuleBottom -= transform.forward * capsuleCollider.radius * 2;

    //     // 45 degrees down from forward
    //     Vector3 dir = (Vector3.down + transform.forward) / 2;

    //     RaycastHit capsuleHit;

    //     if (Physics.CapsuleCast(
    //         capsuleTop,
    //         capsuleBottom,
    //         capsuleCollider.radius,
    //         dir,
    //         out capsuleHit,
    //         capsuleCastCheckDistance,
    //         LayerMask.GetMask("Obstacle", "Ground"),
    //         QueryTriggerInteraction.Ignore
    //         ))
    //     {
    //         // Success!
    //         ledgePoint = capsuleHit.point;
    //         ledgeNormal = capsuleHit.normal;
    //         return new Ledge(ledgePoint, transform.position, false);

    //     }
    //     else
    //     {
    //         Debug.Log("Capsule hit failed");
    //     }

    //     // No ledge found
    //     return new Ledge(Vector3.zero, Vector3.zero, true);
    // }


    // // // Example usage
    // public void DoLedgeCheck()
    // {
    //     Vector3 ledgePoint;
    //     Vector3 ledgeNormal;

    //     if (TryGetLedgeGrabPoint(out ledgePoint, out ledgeNormal))
    //     {
    //         // Assumes the capsule collider's bottom is at the transform's origin

    //         // Move the capsule down so that it meets the ledge at ledgeHangGrabHeight
    //         Vector3 targetPoint = ledgePoint - new Vector3(0, grabHeight, 0);
    //         // Move the target point away from the ledge so our capsule will be flush up against it
    //         targetPoint -= transform.forward * capsuleCollider.radius;

    //         transform.position = targetPoint;
    //         // Look in the opposite direction to the normal
    //         // We want to hang down from the ledge so we strip out the Y component
    //         transform.rotation = Quaternion.LookRotation(new Vector3(-ledgeNormal.x, 0, -ledgeNormal.z));

    //         // Exercise for the reader: Add some easing to make it nice and smooth!
    //     }
    // }
}
