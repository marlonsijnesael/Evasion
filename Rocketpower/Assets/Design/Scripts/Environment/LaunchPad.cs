using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
	GameObject audioObj;

    public float JumpPadVelocity;
	
    private void OnTriggerEnter(Collider other)
    {
        //StartCoroutine(JumpRoutine(other));
		audioObj = GameObject.FindGameObjectWithTag("audio");
		audioObj.GetComponent<GeneralAudio>().JumpPadSound();
        other.gameObject.GetComponent<StateMachine>().jumpMultiplier = JumpPadVelocity;
        other.gameObject.GetComponent<StateMachine>().Jump();
        other.gameObject.GetComponent<StateMachine>().jumpMultiplier = 1.19f;
    }

    private void OnTriggerExit(Collider other)
    {
        //StopCoroutine(JumpRoutine(other));
    }

    IEnumerator JumpRoutine(Collider other)
    {
        float timePassed = 0f;
        other.gameObject.GetComponent<StateMachine>().jumpMultiplier = 2f;
        while (timePassed < .4f)
        {
            timePassed += Time.deltaTime;
            //Vector3 moveDir = (Vector3.up * Time.deltaTime);

            other.gameObject.GetComponent<StateMachine>().Jump();
            Debug.Log("Test");

            yield return new WaitForEndOfFrame();
        }
        other.gameObject.GetComponent<StateMachine>().jumpMultiplier = 1.19f;
    }

}
