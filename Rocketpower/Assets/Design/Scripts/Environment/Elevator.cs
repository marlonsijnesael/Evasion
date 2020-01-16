using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    public FluxManager gm;
    public GameObject Hexagon;
    public Transform startPoint, endPoint;
    public float lerpTime = 3f;
    public bool isElevatorFinished;
    public bool isElevatorRunOnce = true;
    public bool elevatStarted = false;

    IEnumerator MoveElevator(Transform player)
    {
        StateMachine statemachine = player.GetComponent<StateMachine>();
        float time = 0;
        Vector3 start = startPoint.position;
        player.transform.SetParent(Hexagon.transform);
        statemachine.maxSpeed = 0;

        // statemachine.currentMove = statemachine.idleMove;
        // statemachine.currentMove.ExitState(statemachine);
        // statemachine.idleMove.EnterState(statemachine);
        statemachine.animationController.SetBool(statemachine.animator, "B_isIdle", true);
        statemachine.SwitchStates(StateMachine.State.IDLE, statemachine.idleMove);

        statemachine.FOVIdle = 25f;
        yield return new WaitForSeconds(1.5f);
        statemachine.enabled = false;

        while (time < lerpTime)
        {
            Hexagon.transform.position = Vector3.Lerp(start, endPoint.position, time / lerpTime);
            Debug.Log(time / lerpTime);
            time += Time.deltaTime;

            yield return null;
        }
        player.transform.SetParent(null);
        statemachine.enabled = true;
        statemachine.maxSpeed = 9f;
        statemachine.currentMove = statemachine.runMove;
        statemachine.FOVIdle = 70f;
        isElevatorFinished = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gm.readyP1 = true;
            gm.readyToggleP1.isOn = true;
        }

        if (other.transform.tag == "Player2")
        {
            gm.readyP2 = true;
            gm.readyToggleP2.isOn = true;
        }

        if (gm.bothPlayersReady && isElevatorRunOnce)
        {
            elevatStarted = true;
            isElevatorRunOnce = false;
            StartCoroutine(MoveElevator(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gm.readyP1 = false;
            gm.readyToggleP1.isOn = false;
        }

        if (other.transform.tag == "Player2")
        {
            gm.readyP2 = false;
            gm.readyToggleP2.isOn = true;
        }
    }
}