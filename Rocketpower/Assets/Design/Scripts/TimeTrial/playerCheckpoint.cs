using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerCheckpoint : MonoBehaviour
{
    public Transform[] checkpointArray;
    public int currentCheckpoint = 0;
    static Vector3 startPos;
    public int checkpointsLength;
    public GameObject[] checkpoints;

    public static playerCheckpoint _Instance;

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

    void Start()
    {
        checkpointsLength = checkpoints.Length;
        startPos = transform.position;
    }

    void Update()
    {
        
    }

 

}
