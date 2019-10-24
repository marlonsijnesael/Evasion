
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Smooth3rdperson : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float moveSpeed = 5f;
    public float smoothSpeed = 0.5f;
    public float turnSpeed = 5f;
    private Quaternion targetRotation;
    private Vector3 targetPos;
    private bool smoothRotating = false;

    private void Update()
    {
        MoveWithTarget();
        LookAtTarget();
        if (Input.GetKey(KeyCode.A) && !smoothRotating)
        {
            Coroutine coroutine = StartCoroutine(RotateAroundTarget(45));
        }
        if (Input.GetKey(KeyCode.D) && !smoothRotating)
        {
            Coroutine coroutine = StartCoroutine(RotateAroundTarget(-45));
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

    private IEnumerator RotateAroundTarget(float angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle, 0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);

        while (dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
            yield return null;
        }
        smoothRotating = false;
        offsetPos = targetOffsetPos;
    }
}
