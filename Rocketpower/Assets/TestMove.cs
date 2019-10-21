using UnityEngine;
using System.Collections;
public class TestMove : MonoBehaviour
{

    [Header("Acceleration Settings: ")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0;

    [Header("Scene objects: ")]
    [SerializeField] private Camera head;

    [SerializeField] private Transform body;

    [Header("collision settings: ")]
    public TestPhysx collision;
    public static Vector3 velocity = new Vector3();
    private Quaternion targetRot;
    [SerializeField] private Animator anim;

    [SerializeField] private float sensitivity = 5;
    [SerializeField] private float maxLookUpDownAngle = 60; // can't look up/down more than 60 dgs

    private float upDownRotation = 0.0f;
    private float rightLeftRotation = 0.0f;

    public AnimContrller.animations JumpAnimation;

    private Vector3 CalcForward
    {
        get
        {
            if (collision.isGrounded)
            {
                return Vector3.Cross(collision.verticalHit.normal, -head.transform.right);
            }
            return head.transform.forward;
        }
    }


    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    public void Act()
    {
        if (collision.overrideForce)
            return;
        Move(0);
    }

    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    private void CalcPlayerRot()
    {
        if (collision.isGrounded)
        {
            Quaternion newRot = Quaternion.FromToRotation(body.transform.up, collision.verticalHit.normal) * body.transform.rotation;
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, newRot, Time.deltaTime);
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, newRot, Time.deltaTime);
        }

        else
        {
            body.transform.rotation = new Quaternion(0, 0, 0, 0);//Quaternion.Slerp(body.transform.rotation, new Quaternion(0, 0, 0, 0), Time.deltaTime);
            head.transform.rotation = new Quaternion(0, 0, 0, 0);//Quaternion.Slerp(head.transform.rotation, new Quaternion(0, 0, 0, 0), Time.deltaTime);
        }


    }


    public void Move(int _playerID)
    {
        bool moveVertical = Input.GetKey(KeyCode.UpArrow);// InputCatcher.GetAxis("vertical", _playerID);
        anim.SetBool("isRunning", Mathf.Abs(velocity.z) > 0);

        CalcPlayerRot();


        if (!moveVertical)
        {
            Accelerate(decelRatePerSec);
        }

        else
        {
            Accelerate(accelRatePerSec);
        }

        if (!collision.horizontalStop)
        {
            velocity = (CalcForward * forwardVelocity) + collision.downwardForce + collision.upwardForce + collision.stopForce;
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}



// private float GetCalcGroundAngle()
// {
//     if (!collision.isGrounded)
//     {
//         return 0;
//     }
//     return Vector3.Angle(collision.verticalHit.normal, root.transform.forward);
// }


// private IEnumerator SmoothRot(float time, Quaternion newRot)
// {
//     float elapsedTime = 0;
//     while (elapsedTime < time)
//     {
//         Quaternion.Slerp(transform.rotation, newRot, (elapsedTime / time));
//         elapsedTime += Time.deltaTime;

//         yield return new WaitForEndOfFrame();
//     }
//     yield return null;
// }

