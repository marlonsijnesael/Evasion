
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Smooth3rdperson : MonoBehaviour
{
    public UnityEngine.Transform player;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
    }
    // Update is called once per frame
    void Update()
    {
        // Follow Transform
        //transform.position = player.position + offset;
        // Move Towards
        //float speed = 3.0f;
        //transform.position = Vector3.MoveTowards(transform.position, player.position + offset, Time.deltaTime * speed);
        // Linear Interpolation
        //float speed = 1.0f;
        //transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * speed);
        // Smooth Damp
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, 0.1f);
        print(velocity);
    }
}
