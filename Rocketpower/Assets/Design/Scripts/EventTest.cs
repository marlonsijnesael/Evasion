using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventTest : MonoBehaviour
{
    public UnityEvent playerNull;
    public InputCatcher player;


    private void TestRealShit()
    {
        Debug.Log("wow");
    }

    private void Start()
    {
        //if(player == null)
        //{
        //    player = FindObjectOfType<InputCatcher>();
        //}
      
    }

    private void Update()
    {
        //if (player == null)
        //{
        //    playerNull.Invoke();
        //}
    }
}
