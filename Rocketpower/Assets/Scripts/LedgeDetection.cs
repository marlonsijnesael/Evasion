using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    public CharacterController cc;
    public float sphereRadius;
    public float maxDist;
    public float maxHeight;
    public Vector3 origin;
    public Vector3 dir;
    public GameObject currentHitObject;
    public LayerMask layerMask;
    public float currentHitDistance;
    public float heightAboveHead;
    public RaycastHit ledgeHit;

    private void Awake()
    {
        maxHeight = cc.height + heightAboveHead;
    }

    public Ledge CheckLedge()
    {
        origin = transform.position;
        origin.y += maxHeight;
        dir = transform.forward;
        dir.z /= 3;
        RaycastHit spherehit;

        // first check if the player won't hit anything above its head
        if (!Physics.SphereCast(origin, sphereRadius, transform.up, out spherehit))
        {
            // check if the top of the ledge is within reach
            if (!Physics.Raycast(origin + dir, Vector3.down, out ledgeHit, maxDist))
                Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.green);
            else
            {
                return new Ledge(ledgeHit.point, transform.position, false);
            }
        }
        return new Ledge(Vector3.zero, Vector3.zero, true);
    }
}


// Holds information about the ledge, if one is detected
[System.Serializable]
public class Ledge
{
    public Vector3 hitPosition;
    public Vector3 playerPosition;
    public bool empty;
    public Ledge(Vector3 _pos, Vector3 _playerPosition, bool _empty)
    {
        hitPosition = _pos;
        playerPosition = _playerPosition;
        empty = _empty;
    }
}
