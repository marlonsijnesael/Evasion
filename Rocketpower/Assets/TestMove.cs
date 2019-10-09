using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [Header("Acceleration Settings: ")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0;
    private float speed = 5;
    private float moveHorizontal = 0f;

    [Header("Scene objects: ")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform body;
    [SerializeField] private Transform foot;

    [Header("collision settings: ")]
    [SerializeField] private float height = 0.5f;
    public TestPhysx collision;

    public static Vector3 velocity = new Vector3();
    private Quaternion targetRot;
    private Animator anim;
    private float inputX, inputZ;
    private float angle;
   
    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    private void FixedUpdate()
    {
        if (collision.overrideForce)
            return;
        Move(0);
        Rotate();
    }

    private void Rotate()
    {
        body.localRotation = Quaternion.Euler(transform.localEulerAngles.x, playerCam.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    private void CorrectGround()
    {
        if (Vector3.Distance(foot.transform.position, collision.verticalHit.point) < height && collision.isGrounded)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
        }
    }

    private Vector3 CalcForward()
    {
        if (!collision.isGrounded)
        {
            return playerCam.transform.forward;
        }
        return Vector3.Cross(collision.verticalHit.normal, -playerCam.transform.right);

    }

    private float CalcGroundAngle()
    {
        if (!collision.isGrounded)
        {
            return Vector3.Angle(new Vector3(0, 1, 0), foot.transform.forward);
        }
        return Vector3.Angle(collision.verticalHit.normal, foot.transform.forward);
    }

    public void Move(int _playerID)
    {
        bool moveVertical = Input.GetKey(KeyCode.UpArrow);// InputCatcher.GetAxis("vertical", _playerID);
        if (moveVertical)
        {
            Accelerate(accelRatePerSec);
        }

        else
        {
            Accelerate(decelRatePerSec);
        }
        if (!collision.horizontalStop)
        {
            velocity = (CalcForward() * forwardVelocity) + collision.downwardForce + collision.upwardForce + collision.stopForce;
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}


