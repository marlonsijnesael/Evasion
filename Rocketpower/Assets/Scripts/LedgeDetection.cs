using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    public CharacterController cc;
    public float sphereRadius;
    public float maxDist;
    public float maxHeight;
    public Vector3 origin;
    public Vector3 dir;
    public float heightAboveHead;
    public RaycastHit ledgeHit;

    private void Awake()
    {
        maxHeight = cc.height + heightAboveHead;
    }

    private void Update()
    {
        maxHeight = cc.height + heightAboveHead;
        origin = transform.position;
        origin.y += maxHeight;
        dir = transform.forward;
        dir.z *= 2;
        RaycastHit spherehit, forwardHit;

        //FORWARD FROM PLAYER
        if (Physics.Linecast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, out forwardHit))
        {

            Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, Color.red);
            // if (!Physics.SphereCast(origin, sphereRadius, transform.up, out spherehit))
            // {

            // DOWN TO LEDGE
            if (!Physics.Raycast(origin + dir, Vector3.down, out ledgeHit, maxDist))
            {
                Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.green);
            }

            else
            {
                Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.red);
                //UP TO SKY
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up * 2.5f, Color.red);
                if (Physics.Linecast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up))
                {

                }
                else
                {
                    //return new Ledge(ledgeHit.point, transform.position, false);
                }
            }
        }
    }
    //forward
    //down on ledge#
    //up to sky

    public Ledge CheckLedge()
    {
        maxHeight = cc.height + heightAboveHead;
        origin = transform.position;
        origin.y += maxHeight;
        dir = transform.forward;
        dir.z *= 2;
        RaycastHit spherehit, forwardHit;

        //FORWARD FROM PLAYER
        if (Physics.Linecast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, out forwardHit))
        {

            Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, Color.red);
            // if (!Physics.SphereCast(origin, sphereRadius, transform.up, out spherehit))
            // {

            // DOWN TO LEDGE
            if (!Physics.Raycast(origin + dir, Vector3.down, out ledgeHit, maxDist))
            {
                Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.green);
            }

            else
            {
                Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.red);
                //UP TO SKY
                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up * 2.5f, Color.red);
                if (Physics.Linecast(transform.position + Vector3.up, transform.position + Vector3.up))
                {

                }
                else
                {
                    return new Ledge(ledgeHit.point, transform.position, false);
                }
            }
        }
        else
            Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, Color.green);
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
