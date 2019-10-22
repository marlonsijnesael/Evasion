using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class containerCheckpoint : MonoBehaviour
{
    static private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        playerManager._Instance.UpdatecheckPoints(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
