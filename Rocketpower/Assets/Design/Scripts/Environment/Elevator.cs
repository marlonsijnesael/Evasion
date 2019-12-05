using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject Hexagon;

    private void OnTriggerStay(Collider other){
        Hexagon.transform.position += Hexagon.transform.up * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other){
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
