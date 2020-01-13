using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    public GameObject Hexagon;
    public Transform startPoint, endPoint;
    public float lerpTime = 3f;





    private IEnumerator MoveElevator(Transform player)
    {
        player.transform.SetParent(Hexagon.transform);
        player.GetComponent<StateMachine>().enabled = false;
        float dist = endPoint.position.y - startPoint.position.y;
        float timeStep = dist / lerpTime;
        float time = 0;

        while (time < lerpTime)
        {
            Hexagon.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, time / lerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.SetParent(null);
        player.GetComponent<StateMachine>().enabled = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        // other.transform.SetParent(this.transform);
        StartCoroutine(MoveElevator(other.transform));
    }

    void Update()
    {

    }
}
