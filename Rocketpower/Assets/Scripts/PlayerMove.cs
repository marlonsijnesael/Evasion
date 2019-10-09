using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform body;
    [SerializeField] private Transform foot;
    public LayerMask ground;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0;
    private float speed = 5;
    private float moveHorizontal = 0f;
    private float prevAngle = 0;
    private float maxJumpHeight = 10f;
    private bool inputJump = false;
    private float jumpSpeed = 0.1f;


    private bool grounded = false;
    [SerializeField] private float padding = 0.05f;
    [SerializeField] private float height = 0.5f;
    public float maxGroundAngle = 120;
    public bool debug = false;

    private RaycastHit hitInfo;

    public Animator anim;

    private float inputX, inputZ;
    private float angle;
    private Quaternion targetRot;
    public static Vector3 velocity = new Vector3();

    public RayCollider collision;

    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
        RayCollider.OnCollisionRegistered += OnCollisionHorizontalRegistered;
    }

    private void OnCollisionHorizontalRegistered(RaycastHit _hit)
    {
        print("hit hit hit" + _hit.transform.name);

    }

    private void Rotate()
    {
        //transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, playerCam.transform.localEulerAngles.y, transform.localEulerAngles.z);
        body.localRotation = Quaternion.Euler(transform.localEulerAngles.x, playerCam.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    //private void CheckGround()
    //{
    //    if (Physics.Raycast(foot.transform.position, Vector3.down, out hitInfo, height + padding, ground))
    //    {
    //        CorrectGround();
    //        grounded = true;
    //    }
    //    else
    //    {
    //        grounded = false;
    //    }
    //}

    private void CorrectGround()
    {
        if (Vector3.Distance(transform.position, hitInfo.point) < height)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        transform.position += Physics.gravity * Time.deltaTime;
    }

    private Vector3 CalcForward()
    {
        if (!collision.isGrounded)
        {
            return transform.forward;
        }
        return Vector3.Cross(foot.transform.right, hitInfo.normal);

    }

    private float CalcGroundAngle()
    {
        if (!collision.isGrounded)
        {
            return Vector3.Angle(new Vector3(0, 1, 0), foot.transform.forward);
        }
        return Vector3.Angle(hitInfo.normal, foot.transform.forward);
    }

    private void DebugRay()
    {
        if (debug)
        {
            Debug.DrawLine(foot.transform.position, foot.transform.position + CalcForward() * height * 2, Color.blue);
        }
    }

    public void Move(int _playerID)
    {
        bool moveVertical = Input.GetKey(KeyCode.UpArrow);// InputCatcher.GetAxis("vertical", _playerID);
        if (Input.GetKeyDown(KeyCode.Space) && collision.isGrounded)
        {
            inputJump = true;
            print("ok");
            StartCoroutine(Jump());
        }
        Rotate();
        DebugRay();

        //CheckGround();
        if (!collision.isGrounded)
        {
            ApplyGravity();
        }

        if (moveVertical)
        {
            Accelerate(accelRatePerSec);
        }

        else
        {
            Accelerate(decelRatePerSec);
        }

        velocity = CalcForward() * forwardVelocity * Time.deltaTime;
        transform.Translate(velocity);
    }


    IEnumerator Jump()
    {
        Vector3 groundPos = transform.position;
        float maxJumpH = maxJumpHeight + transform.position.y;
        while (inputJump)
        {
            transform.Translate(Vector3.up * jumpSpeed * Time.smoothDeltaTime);
            if (transform.position.y > maxJumpH)
            {
                //transform.position = groundPos;
                //StopAllCoroutines();
                inputJump = false;
            }
        }

        if (!inputJump)
        {
            transform.Translate(Vector3.down * jumpSpeed * Time.smoothDeltaTime);
            if (transform.position.y < groundPos.y)
            {
                transform.position = groundPos;
                StopAllCoroutines();
            }
        }

        yield return new WaitForEndOfFrame();
    }
}



