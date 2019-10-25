using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class startTimer : MonoBehaviour
{
    public static startTimer _Instance;

    public GameObject TextUI;
    public GameObject CheckpointUI;

    public Text timerLabel;
    public float time;

    public bool isTimer;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Start")
        {
            time = 0;
            isTimer = true;
            TextUI.SetActive(true);
            CheckpointUI.SetActive(true);
            playerManager._Instance.ResetCheckpoints();
            //other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Fin" && playerManager._Instance.isFinishActive)
        {
            Debug.Log("Finish Time");
            isTimer = false;
            timerLabel.color = Color.green;
        }
    }

    private void Update()
    {
        if (isTimer)
        {
            time += Time.deltaTime;

            var minutes = time / 60;
            var seconds = time % 60;
            var fraction = (time * 100) % 100;

            timerLabel.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
            timerLabel.color = Color.white;
        }
    }

}
