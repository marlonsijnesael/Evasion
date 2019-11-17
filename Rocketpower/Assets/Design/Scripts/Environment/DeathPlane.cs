using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawn;


    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("Hit");
        // player.transform.position = respawn.transform.position;
        // if (startTimer._Instance.isTimer == false)
        // {
        //     startTimer._Instance.time = 0;
        // }
        // else
        // {
        //     startTimer._Instance.timerLabel.color = Color.red;
        //     startTimer._Instance.isTimer = false;
        // }
    }

}
