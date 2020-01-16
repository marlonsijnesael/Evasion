using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLightRing : MonoBehaviour
{
 public GameObject player;
 private float yPos;
 private float xPos;
 private float zPos;
    // Start is called before the first frame update
    void Start()
    {
    
       xPos = gameObject.transform.position.x;
       zPos = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        yPos = player.transform.position.y;

        gameObject.transform.position = new Vector3(xPos,yPos,zPos);
       
    }
}
