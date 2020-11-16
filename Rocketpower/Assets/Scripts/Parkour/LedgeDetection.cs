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
    public float climbAngle = 150;

    public string nameObj = string.Empty;

    private void Awake()
    {
        maxHeight = cc.height + heightAboveHead;
    }

    private void Update()
    {
        // Debug.Log(GettotalHeight() + " total height");
    }

    //private void Update()
    //{
    //    maxHeight = cc.height + heightAboveHead;
    //    origin = transform.position;
    //    origin.y += maxHeight;
    //    dir = transform.forward;
    //    dir.z *= 2;
    //    RaycastHit spherehit, forwardHit;

    //    //FORWARD FROM PLAYER
    //    if (Physics.Linecast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, out forwardHit))
    //    {

    //        Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, Color.red);
    //        // if (!Physics.SphereCast(origin, sphereRadius, transform.up, out spherehit))
    //        // {

    //        // DOWN TO LEDGE
    //        if (!Physics.Raycast(origin + dir, Vector3.down, out ledgeHit, maxDist))
    //        {
    //            Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.green);
    //        }

    //        else
    //        {
    //            Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.red);
    //            //UP TO SKY
    //            Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up * 2.5f, Color.red);
    //            if (Physics.Linecast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up))
    //            {

    //            }
    //            else
    //            {
    //                //return new Ledge(ledgeHit.point, transform.position, false);
    //            }
    //        }
    //    }
    //}
    //forward
    //down on ledge#
    //up to sky

    public Ledge GetLedgeV2()
    {
        RaycastHit fwdHit;

        float totalHeight = 0;
        float playerHeight = 1.80f;
        Vector3 tmpPos = Vector3.zero;
        Vector3 origin = transform.position + (Vector3.up * 2);
        float maxLedgeHeight = 10;

        // Debug.DrawLine(origin, origin + (transform.forward * 1.5f), Color.green);

        //check if there is an object in front of the player
        if (Physics.Raycast(origin, transform.forward, out fwdHit, 1.5f))
        {
            if (fwdHit.transform.tag == "noclimb")
            {
                Debug.Log("no climb");
                return new Ledge(Vector3.zero, Vector3.one, Vector3.zero, true);
            }
            if (fwdHit.transform.tag == "outer")
            {
                Debug.Log("outer wall");
                return new Ledge(Vector3.up * 300, transform.position, Vector3.up, false);

            }
            // Debug.DrawLine((Vector3.up * maxLedgeHeight) + origin, transform.position + transform.forward, Color.blue);
            RaycastHit topHit;
            //check if the top of the object is range of the player's max ledge height 
            if (Physics.Raycast((Vector3.up * maxLedgeHeight) + transform.position + transform.forward, Vector3.down, out topHit, 10))
            {
                //check if there is room for the player to stand on top of the object
                if (!Physics.Raycast(topHit.point, Vector3.up, playerHeight + 0.2f))
                {
                    // Debug.DrawLine(topHit.point, topHit.point + (Vector3.up * playerHeight), Color.green);
                    return new Ledge(topHit.point, transform.position, topHit.normal, false);
                }
            }

        }
        return new Ledge(Vector3.zero, Vector3.one, Vector3.zero, true);
    }


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
            float cosine = Vector3.Angle(transform.forward, forwardHit.normal);
            // if (cosine < climbAngle)
            //     return new Ledge(Vector3.zero, Vector3.one, Vector3.zero, true);

            Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, Color.green);
            if (forwardHit.transform.CompareTag("holowall"))
            {
                nameObj = ledgeHit.transform.name;
                return new Ledge(transform.position + (Vector3.up), Vector3.zero, transform.position, false);
            }
            else
            {
                // DOWN TO LEDGE
                if (!Physics.Raycast(origin + dir, Vector3.down, out ledgeHit, maxDist))
                {
                    Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.green);
                    nameObj = ledgeHit.transform.name;
                }

                else
                {
                    Debug.DrawRay(origin + dir, Vector3.down * maxDist, Color.red);
                    //UP TO SKY
                    Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up * 2.5f, Color.green);
                    if (Physics.Linecast(transform.position + Vector3.up, transform.position + Vector3.up))
                    {
                        nameObj = ledgeHit.transform.name;
                    }
                    else
                    {

                        nameObj = ledgeHit.transform.name;
                        return new Ledge(ledgeHit.point, ledgeHit.normal, transform.position, false);
                    }
                }
            }
        }
        else
            Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, Color.green);
        return new Ledge(Vector3.zero, Vector3.one, Vector3.zero, true);
    }
}



// Holds information about the ledge, if one is detected
[System.Serializable]
public class Ledge
{
    public Vector3 hitPosition;
    public Vector3 playerPosition;
    public Vector3 normal;
    public bool empty;

    public Ledge(Vector3 _pos, Vector3 _playerPosition, Vector3 _normal, bool _empty)
    {
        hitPosition = _pos;
        playerPosition = _playerPosition;
        normal = _normal;
        empty = _empty;
    }
}
