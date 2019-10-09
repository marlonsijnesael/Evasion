using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerNEW : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float turnSpeed = 10;
    public float smoothSpeed = 0.5f;
    public float moveSpeed = 5;

    private Quaternion targetRotation;
    private Vector3 targetPos;
    private bool smoothRotate;

    private void Update()
    {
        MoveWithTarget();
        LookAtTarget();
        if (Input.GetKey(KeyCode.G) && !smoothRotate)
        {
            StartCoroutine("RotateAroundTarget", 45);
        }

        if (Input.GetKey(KeyCode.H) && !smoothRotate)
        {
            StartCoroutine("RotateAroundTarget", -45);
        }

    }

    private void MoveWithTarget()
    {
        targetPos = target.position + offsetPos;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private IEnumerator RotateAroundTarget(float _angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, _angle, 0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);
        smoothRotate = true;

        while (dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
            yield return null;
        }

        smoothRotate = false;
        offsetPos = targetOffsetPos;

    }

}
