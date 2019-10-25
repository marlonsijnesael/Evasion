using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public int JumpVelocity = 30;
    private void OnTriggerEnter(Collider other) {
        StartCoroutine(JumpRoutine(other));       
    }

    private void OnTriggerExit(Collider other) {
        StopCoroutine(JumpRoutine(other));
    }

    IEnumerator JumpRoutine(Collider other) {
        float timePassed = 0f;
        while ( timePassed < 2f) {
            timePassed += Time.deltaTime;
            Vector3 moveDir = (Vector3.up * Time.deltaTime);

            other.gameObject.GetComponent<CharacterController>().Move(moveDir * JumpVelocity);
                
            

            yield return new WaitForEndOfFrame();
        }        
    }

}
