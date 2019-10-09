using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewController : MonoBehaviour
{
    public float velocity = 5;
    public float turnSpeed = 10;

    private Vector2 input;
    private float angle;

    private Quaternion targetRotation;
    private Transform cam;
    private string player;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float timeZeroToMax = 2.5f;
    [SerializeField] private float timeMaxToZero = 2.5f;
    [SerializeField] private float padding = 0.05f;
    [SerializeField] private float height = 0.5f;

    private bool grounded = false;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0;

    public float maxGroundAngle = 120;
    public bool debug = false;

    private RaycastHit hitInfo;

    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        GetInput(player);

        //if (Mathf.Abs(input.x) < 1  && Mathf.Abs(input.y) < 1)
        //{
        //    return;
        //}
        CalculateAngle();
        Rotate();
        Move();
    }

    private void GetInput(string _axis)
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void CalculateAngle()
    {
        if (Mathf.Abs(input.x) < 0) return;
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        //angle += cam.transform.eulerAngles.y;
    }

    private void Rotate()
    {
        if (Mathf.Abs(input.x) < 0) return;
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        //transform.position += transform.forward * velocity * Time.deltaTime;
        bool moveVertical = Input.GetKey(KeyCode.UpArrow) ; //InputCatcher.GetAxis("vertical", _playerID);

        CheckGround();
        ApplyGravity();
        //Rotate();
        if (CalcGroundAngle() <= maxGroundAngle)
        {
            if (moveVertical)
            {
                Accelerate(accelRatePerSec);
            }

            else
            {
                Accelerate(decelRatePerSec);
            }
            //anim.SetFloat("ForwardVelocity", forwardVelocity);
            transform.Translate(CalcForward() * forwardVelocity * Time.deltaTime);
        }
    }


    private void Accelerate(float rate)
    {
        forwardVelocity += rate * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, height + padding))
        {
            print(hitInfo.transform.tag);

            if (hitInfo.transform.CompareTag("Ground"))
            {
                CorrectGround();
                grounded = true;
            }
        }
        else
        {
            grounded = false;
        }
    }

    private void CorrectGround()
    {
        if (Vector3.Distance(transform.position, hitInfo.point) < height)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    private Vector3 CalcForward()
    {
        if (!grounded)
        {
            return transform.forward;
        }
        return Vector3.Cross(transform.right, hitInfo.normal);

    }

    private float CalcGroundAngle()
    {
        if (!grounded)
        {
            return 0;
        }
        return Vector3.Angle(hitInfo.normal, transform.forward);
    }




}
