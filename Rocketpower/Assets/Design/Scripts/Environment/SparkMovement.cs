using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkMovement : MonoBehaviour
{
    private Vector3 startPoint;
    public float length;
    public float speed;
    void Start()
    {
        startPoint = this.transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(startPoint.x + Mathf.PingPong(Time.time * speed, length), startPoint.y, startPoint.z);
    }
}
