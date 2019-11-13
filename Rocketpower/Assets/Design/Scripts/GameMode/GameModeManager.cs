using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;

    public GameObject fluxPlayer;

    public int orangeScore;
    public int blueScore;

    GameObject[] platformArray;

    List<GameObject> platformList;

    public Material m_orangePlatform;
    public Material m_bluePlatform;

    void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            updateScore();
        }
    }

    public void updateScore() {
        foreach (GameObject platform in platformArray) {
            if(platform.gameObject.GetComponent<Renderer>().material.name == "M_PlatformOrange_v1 (Instance)") {
                Debug.Log(platform);
            }
            if(platform.gameObject.GetComponent<Renderer>().material.name == "M_PlatformBlue_v1 (Instance)") {
                Debug.Log(platform);
                List<GameObject> blueList = new List<GameObject>();
            }
        }

        //List<GameObject> orangeList = new List<GameObject>();

        //for (int i = 0; i < platformArray.Length; i++) {
        //    orangeList.Add(platform);
        //}
        //orangeScore = orangeList.Count;
        //Debug.Log(orangeScore);
    }
}
