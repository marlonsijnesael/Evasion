using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZRotate : MonoBehaviour
{
    public EzCamera ezCam;

    [SerializeField] private float m_rotateSpeed;
    [SerializeField] private Vector3 m_moveVector;
    [SerializeField] private VirtualController vC;

    private void LateUpdate()
    {
        Rotate();
    }

    public void Rotate()
    {
        Vector3 inputVector = ezCam.ConvertMoveInputToCameraSpace(vC.HorizontalMovement, vC.VerticalMovement);

        if (vC.HorizontalMovement != 0 || vC.VerticalMovement != 0)
        {
            print("rotating");
            float step = m_rotateSpeed * Time.deltaTime;
            inputVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(inputVector, Vector3.up);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, step);
            // Debug.DrawRay(transform.position, transform.forward,Color.red, 10f);
        }
    }
}
