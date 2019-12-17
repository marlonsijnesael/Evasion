using UnityEngine;
using System.Collections;
using System;

public class EzOrbitState : EzCameraState
{
    private float m_rotY = 0; // Camera's current rotation around the X axis (up/down)
    private float m_rotX = 0; // Camera's current rotation around the Y axis (left/right)

    private float m_horizontalInput = 0;
    private float m_verticalInput = 0;

    Quaternion m_destRot = Quaternion.identity;

   

    public EzOrbitState(EzCamera camera, EzCameraSettings settings)
        : base(camera, settings) { }


    public override void EnterState()
    {
        //
    }

    public override void ExitState()
    {
        //
    }

    public override void UpdateState()
    {
        if (m_controlledCamera.OribtEnabled)
        {
            HandleInput();
        }
    }

    public override void LateUpdateState()
    {
        OrbitCamera(m_horizontalInput, m_verticalInput);
    }

    public override void UpdateStateFixed()
    {
        //
    }

    private void OrbitCamera(float horz, float vert)
    {
        // cache the step and update the roation from input
        float step = Time.deltaTime * m_stateSettings.RotateSpeed;
        m_rotY += horz * step; 
        m_rotX += vert * step;
        m_rotX = Mathf.Clamp(m_rotX, m_stateSettings.MinRotX, m_stateSettings.MaxRotX);
        
        // compose the quaternions from Euler vectors to get the new rotation
        Quaternion addRot = Quaternion.Euler(0f, m_rotY, 0f);
        m_destRot = addRot * Quaternion.Euler(m_rotX, 0f, 0f); // Not commutative

        m_cameraTransform.rotation = m_destRot;

#if UNITY_EDITOR
        Debug.DrawLine(m_cameraTransform.position, m_cameraTarget.transform.position, Color.red);
        Debug.DrawRay(m_cameraTransform.position, m_cameraTransform.forward, Color.blue);
#endif

        m_controlledCamera.UpdatePosition();
    }

    public void ResetBehindPlayer()
    {
       Vector3 targetPos = m_cameraTarget.position + ((m_cameraTarget.up * m_stateSettings.OffsetHeight) + (m_cameraTarget.right * m_stateSettings.LateralOffset) + (m_cameraTarget.forward * -m_stateSettings.OffsetDistance));
        m_cameraTransform.position = targetPos;

        Vector3 relativePos = (m_cameraTarget.position + (Vector3.right * m_stateSettings.LateralOffset) + (Vector3.up * m_stateSettings.OffsetHeight)) - m_cameraTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(relativePos);
        m_cameraTransform.rotation = lookRotation;
    }

    public override void HandleInput()
    {// cache the inputs
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBehindPlayer();
            return;
        }

        float horz = this.m_cameraTarget.gameObject.GetComponent<VirtualController>().HorizontalLook;
        float vert = this.m_cameraTarget.gameObject.GetComponent<VirtualController>().VerticalLook;

        if (m_controlledCamera.OribtEnabled && (Mathf.Abs(horz) > 0 || Mathf.Abs(vert) > 0))
        {
            m_controlledCamera.SetState(State.ORBIT);
        }
        else if ((Mathf.Abs(horz) < 0 && Mathf.Abs(vert) < 0))
        {
            m_controlledCamera.SetState(State.FOLLOW);
            return;
        }

        m_horizontalInput = horz;
        m_verticalInput = vert;
    }
}
